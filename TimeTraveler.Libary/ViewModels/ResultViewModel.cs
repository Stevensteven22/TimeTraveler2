using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TimeTraveler.Libary.Definitions;
using TimeTraveler.Libary.Helpers;
using TimeTraveler.Libary.Models;
using TimeTraveler.Libary.Services;
using Ursa.Controls;

namespace TimeTraveler.Libary.ViewModels;

public partial class ResultViewModel : ViewModelBase
{
    private bool _isDialogShowed = false;
    private int _currentStep = 0;

    public ObservableCollection<ResultModel> _resultModels { get; private set; } =
        new ObservableCollection<ResultModel>();

    private readonly IElementalService _elementalService;

    [ObservableProperty]
    private string _result;

    [RelayCommand]
    private void GoToReturnView()
    {
        _currentStep++;
        if (_currentStep >= 3)
        {
            if (_currentStep == 3)
            {
                //游戏已完成3关时候，可做TODO：（试炼界面显示可返回按钮）
                WeakReferenceMessenger.Default.Send<object, string>(
                    new object(),
                    "OnGameCompleted"
                );
            }

            //如果超过3关，则随机抽取ResultModels的3个子项组成新的集合
            if (_currentStep > 3)
            {
                var random = new Random();
                var selectedItems = _resultModels.OrderBy(x => random.Next()).Take(3).ToList();
                _resultModels = new ObservableCollection<ResultModel>(selectedItems);
            }

            if (!_isDialogShowed)
                ShowDialog();
            else
                WeakReferenceMessenger.Default.Send<object, string>(0, "OnForwardNavigation");
        }
        else
        {
            WeakReferenceMessenger.Default.Send<object, string>(0, "OnForwardNavigation");
        }
    }

    public ResultViewModel(IElementalService elementalService)
    {
        _elementalService = elementalService;
        OnLoadedCommand = new AsyncRelayCommand(OnLoadedAsync);
        WeakReferenceMessenger.Default.Register<object, string>(
            this,
            "OnResultSubmitted",
            (sender, message) =>
            {
                if (message is ElementType elementType)
                {
                    IsOK = false;
                    switch (elementType)
                    {
                        case ElementType.FireElemental:
                            Result = "火元素力量正在爆发他的火焰力量，火元素的力量是无穷无尽的...";
                            _resultModels.Add(
                                new ResultModel()
                                {
                                    Id = 0,
                                    Name = "火元素",
                                    ResultElementType = ElementType.FireElemental,
                                    IsSelected = false,
                                    ImprovedValue2 = 10,
                                    Description = "火元素之力: 攻击力+10%。",
                                }
                            );
                            break;
                        case ElementType.IceElemental:
                            Result =
                                "冰元素之力正在释放，冰霜慢慢融化，冰元素的力量让生命开始复苏...";
                            _resultModels.Add(
                                new ResultModel()
                                {
                                    Id = 1,
                                    Name = "冰元素",
                                    ResultElementType = ElementType.IceElemental,
                                    IsSelected = false,
                                    ImprovedValue2 = 10,
                                    Description = "冰元素之力: 生命力+10%。",
                                }
                            );
                            break;
                        case ElementType.WindElemental:
                            Result =
                                "风元素之力正在扩散，一股力量快速涌动，与时间赛跑，风之力量让时间停止流逝...";
                            _resultModels.Add(
                                new ResultModel()
                                {
                                    Id = 2,
                                    Name = "风元素",
                                    ResultElementType = ElementType.WindElemental,
                                    IsSelected = false,
                                    ImprovedValue1 = 10,
                                    Description = "风元素之力: 闪避率+10%。",
                                }
                            );
                            break;
                        case ElementType.RockElemental:
                            Result =
                                "岩元素之力正在聚集中辽阔的大地各方的金光，岩元素的力量让大地变得更加坚固无比...";
                            _resultModels.Add(
                                new ResultModel()
                                {
                                    Id = 3,
                                    Name = "岩元素",
                                    ResultElementType = ElementType.RockElemental,
                                    IsSelected = false,
                                    ImprovedValue2 = 10,
                                    Description = "岩元素之力: 防御力+10%。",
                                }
                            );
                            break;
                        case ElementType.ThunderElemental:
                            IsOK = true;
                            Result =
                                "雷元素之力正在收缩，在天空刹那间，雷电凝聚形成一道瞬雷，像刀刃般锋芒，刀光剑影，劈开了大山，雷元素的力量让人惊心动魄...";
                            _resultModels.Add(
                                new ResultModel()
                                {
                                    Id = 4,
                                    Name = "雷元素",
                                    ResultElementType = ElementType.ThunderElemental,
                                    IsSelected = false,
                                    ImprovedValue1 = 10,
                                    Description = "雷元素之力: 暴击率+10%。",
                                }
                            );
                            break;
                    }
                }
            }
        );

        WeakReferenceMessenger.Default.Register<object, string>(
            this,
            "OnResultConfirmed",
            (sender, message) =>
            {
                if (message is bool isConfirmed && isConfirmed)
                {
                    //处理结果时候更新时间轴
                    WeakReferenceMessenger.Default.Send<object, string>(
                        new object(),
                        "OnGameSucceed"
                    );
                    //更新数据库的数据(根据元素添加Buff)
                    _elementalService.InsertOrUpdateElementalAsync(_resultModels);
                }
            }
        );

        WeakReferenceMessenger.Default.Register<object, string>(
            this,
            "OnRestarted",
            (r, p) =>
            {
                OnRestarted();
            }
        );
    }

    private void OnRestarted()
    {
        InitializeGameResult();
    }

    private void ShowDialog()
    {
        WeakReferenceMessenger.Default.Send<object, string>(_resultModels, "ShowMessage");
        _isDialogShowed = true;
    }

    private ICommand OnLoadedCommand { get; }

    [ObservableProperty]
    private bool _isOK;

    public async Task OnLoadedAsync()
    {
        await Task.CompletedTask;
    }

    public void InitializeGameResult()
    {
        _isDialogShowed = false;
        _currentStep = 0;
        _resultModels.Clear();
    }
}
