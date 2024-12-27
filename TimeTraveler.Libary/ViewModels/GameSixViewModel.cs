using Avalonia.Platform;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using NAudio.Wave;
using TimeTraveler.Libary.Services;

namespace TimeTraveler.Libary.ViewModels;

public partial class GameSixViewModel : ViewModelBase
{
    private WaveOutEvent _waveOutEvent;
    private Mp3FileReader _mp3FileReader;
    private readonly IGameTBSService _gameService;
    private readonly IElementalService _elementalService;

    [ObservableProperty]
    private string _characterDeductHPText = string.Empty;

    [ObservableProperty]
    private string _bossDeductHPText = string.Empty;

    [ObservableProperty]
    private double _bossMaxHP = 1000;

    [ObservableProperty]
    private double _bossCurrentHP = 1000;

    [ObservableProperty]
    private double _characterMaxHP = 500;

    [ObservableProperty]
    private double _characterCurrentHP = 500;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AttackCommand))]
    private bool _isCharacterAttacking;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AttackCommand))]
    private bool _isBossAttacking;

    private bool _isBossDefeated = false;
    private bool _isCharacterDefeated = false;

    private bool _isBossKnockedBackCompleted = true;
    public bool IsBossKnockedBackCompleted
    {
        get => _isBossKnockedBackCompleted;
        set
        {
            SetProperty(ref _isBossKnockedBackCompleted, value);

            if (IsCharacterAttacking)
            {
                //人物进攻完毕，通知游戏服务，Boss可以开始自动攻击
                IsCharacterAttacking = false;
                if (_isBossDefeated)
                {
                    // Boss已经被击败，通知游戏服务，游戏结束
                    WeakReferenceMessenger.Default.Send(new object(), "OnBossDefeated");
                    _gameService.Dispose();
                    StopToPlayBackgroundSound();
                    //处理结果时候更新时间轴
                    WeakReferenceMessenger.Default.Send<object, string>(
                        new object(),
                        "OnGameSucceed"
                    );
                    _isBossDefeated = false;
                }
                else
                {
                    _gameService.Produce(new object());
                }
            }
        }
    }

    private bool _isCharacterKnockedBackCompleted = true;
    public bool IsCharacterKnockedBackCompleted
    {
        get => _isCharacterKnockedBackCompleted;
        set
        {
            SetProperty(ref _isCharacterKnockedBackCompleted, value);

            if (IsBossAttacking)
            {
                IsBossAttacking = false;
                if (_isCharacterDefeated)
                {
                    // 人物已经被击败，通知游戏服务，游戏结束
                    WeakReferenceMessenger.Default.Send(new object(), "OnCharacterDefeated");
                    _gameService.Dispose();
                    StopToPlayBackgroundSound();
                    _isCharacterDefeated = false;
                }
            }
        }
    }

    public GameSixViewModel(IGameTBSService gameService, IElementalService elementalService)
    {
        _gameService = gameService;
        _elementalService = elementalService;
        // 注册游戏服务的Boss进击时候的事件处理程序
        _gameService.OnConsumed += ToBossAttack;
        Initialize();

        this.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(BossCurrentHP) && BossCurrentHP <= 0)
            {
                // Boss死亡
                _isBossDefeated = true;
            }
            else if (e.PropertyName == nameof(CharacterCurrentHP) && CharacterCurrentHP <= 0)
            {
                // 角色死亡
                _isCharacterDefeated = true;
            }
        };
    }

    private double _characterHP,
        _characterATK,
        _characterCRI,
        _characterDOD;

    private const double FixedATK = 200.0d,
        FixedCharacterHP = 100.0d;

    private async void Initialize()
    {
        var resultModel = await _elementalService.GetElementalAsync(x => x.Name == "冰元素");
        if (resultModel != null)
        {
            _characterHP =
                resultModel.ActualValue2 == 0.0d
                    ? FixedCharacterHP
                    : FixedCharacterHP * (1.0d + (resultModel.ActualValue2 / 100.0d));
            _characterHP = Math.Round(_characterHP);
            CharacterCurrentHP = _characterHP;
            CharacterMaxHP = _characterHP;
        }

        resultModel = await _elementalService.GetElementalAsync(x => x.Name == "火元素");
        if (resultModel != null)
        {
            _characterATK =
                resultModel.ActualValue2 == 0.0d
                    ? FixedATK
                    : FixedATK * (1.0d + (resultModel.ActualValue2 / 100.0d));
            _characterATK = Math.Round(_characterATK);
        }
        resultModel = await _elementalService.GetElementalAsync(x => x.Name == "风元素");
        if (resultModel != null)
            _characterCRI = resultModel.ActualValue1 == 0.0d ? 15.0d : resultModel.ActualValue1;
        resultModel = await _elementalService.GetElementalAsync(x => x.Name == "雷元素");
        if (resultModel != null)
            _characterDOD = resultModel.ActualValue1 == 0.0d ? 15.0d : resultModel.ActualValue1;
    }

    private const double FixedBossATK = 30.0d;

    private void ToBossAttack(object? sender, EventArgs e)
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            IsBossAttacking = true;
            var random = new Random();
            if (random.Next(0, 100) >= _characterCRI) // _characterCRI命中概率
            {
                CharacterCurrentHP -= FixedBossATK; // 受到boss攻击伤害
                CharacterDeductHPText = $"受到{FixedBossATK}点的伤害！";
                WeakReferenceMessenger.Default.Send(new object(), "OnBossAttacked");
            }
            else
            {
                WeakReferenceMessenger.Default.Send(new object(), "OnBossAttacking");
                CharacterDeductHPText = "闪避了攻击！";
                WeakReferenceMessenger.Default.Send((object)"Character", "TextFlyout");
                IsBossAttacking = false;
            }
        });
    }

    private void PlayBackgroundSound()
    {
        _mp3FileReader?.Dispose();
        _waveOutEvent?.Dispose();

        var stream = AssetLoader.Open(new Uri("avares://TimeTraveler/Assets/关卡4背景音乐.mp3"));
        // 加载并播放 MP3 音效
        _mp3FileReader = new Mp3FileReader(stream);
        _waveOutEvent = new WaveOutEvent();
        _waveOutEvent.Init(_mp3FileReader);
        _waveOutEvent.Play();
    }

    private void StopToPlayBackgroundSound()
    {
        _waveOutEvent?.Stop();
        _mp3FileReader?.Dispose();
        _waveOutEvent?.Dispose();
    }

    [RelayCommand]
    private void Loaded()
    {
        PlayBackgroundSound();
    }

    [RelayCommand]
    private void Unloaded()
    {
        StopToPlayBackgroundSound();
    }

    bool CanAttack() => !(IsCharacterAttacking || IsBossAttacking);

    [RelayCommand(CanExecute = nameof(CanAttack))]
    private void Attack()
    {
        IsCharacterAttacking = true;
        var random = new Random();
        if (random.Next(0, 100) < _characterDOD)
        {
            BossCurrentHP -= _characterATK * 1.5d;
            CharacterDeductHPText = "发动了暴击！";
            BossDeductHPText = $"受到{_characterATK * 1.5d}点的伤害！";
            WeakReferenceMessenger.Default.Send((object)"Character", "TextFlyout");
        }
        else
        {
            BossCurrentHP -= _characterATK;
            BossDeductHPText = $"受到{_characterATK}点的伤害！";
        }

        WeakReferenceMessenger.Default.Send(new object(), "OnCharacterAttacked");
    }
}
