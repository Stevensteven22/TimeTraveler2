using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Messaging;
using TimeTraveler.Libary.Helpers;
using TimeTraveler.Libary.Models;
using TimeTraveler.Libary.Services;

namespace TimeTraveler.Libary.ViewModels;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


public class GameTwoViewModel : ViewModelBase
{
    private readonly IMazeService _mazeService;
    private readonly IBuffStorage _buffStorage;
    private readonly ILargeModelService _largeModelService;
    private string _gameStatus;
    private string _winStatus;
    private string _maze;
    public int _playerX;
    public int _playerY;
    public bool isOK = false;
    private List<Buff> List = new List<Buff>();
    private string theme = null;
    private Puzzle _currentPuzzle;
    private string _reply;

    public string Reply
    {
        get => _reply;
        set => SetProperty(ref _reply, value);
    } 
    public Puzzle CurrentPuzzle
    {
        get => _currentPuzzle;
        set => SetProperty(ref _currentPuzzle, value);
    }

    private string _playerAnswer;

    public string PlayerAnswer
    {
        get => _playerAnswer;
        set => SetProperty(ref _playerAnswer, value);
    }

    public bool _IsPopupVisible = false;

    public Boolean IsPopupVisuale
    {
        get => _IsPopupVisible;
        set => SetProperty(ref _IsPopupVisible, value);
    }


    public HashSet<char> _playerAttributes = new HashSet<char>();
    public ObservableCollection<ObservableCollection<MazeCell>> MazeGrid { get; }
        = new ObservableCollection<ObservableCollection<MazeCell>>(); // 迷宫格子集合

    public int RowCount { get; } = 15; // 固定为 10 行
    public int ColumnCount { get; } = 15; // 固定为 10 列

    private readonly MusicPlayer _musicPlayer = new MusicPlayer();

    public string Maze
    {
        get => _maze;
        set => SetProperty(ref _maze, value);
    }

    public string GameStatus
    {
        get => _gameStatus;
        set
        {
            if (SetProperty(ref _gameStatus, value))
            {
                OnGameStatusChanged();
            }
        }
    }
    public string WinStatus
    {
        get => _winStatus;
        set
        {
            if (SetProperty(ref _winStatus, value))
            {
                OnWinStatusChanged();
            }
        }
    }
    
    private void OnGameStatusChanged()
    {
        if (!string.IsNullOrEmpty(GameStatus))
        {
            ShowGameStatusPopup(GameStatus);
        }
    }
    
    private void OnWinStatusChanged()
    {
        if (!string.IsNullOrEmpty(WinStatus))
        {
            ShowWinStatusPopup(WinStatus);
        }
    }

// 显示弹窗的方法
    private void ShowGameStatusPopup(string message)
    {
        WeakReferenceMessenger.Default.Send(new GameStatusMessage { Message = message });
    }
    private void ShowWinStatusPopup(string message)
    {
        WeakReferenceMessenger.Default.Send(new WinStatusMessage { Message = message });
    }

    //弹窗控制
    private void changeviable()
    {
        this.IsPopupVisuale = true;
    }
    public void close()
    {
        this.IsPopupVisuale = false;
    }
    
    
    public IRelayCommand ChangeCommand { get; }
    public IRelayCommand ClosePopCommand { get; }
    public IRelayCommand MoveUpCommand { get; }
    public IRelayCommand MoveDownCommand { get; }
    public IRelayCommand MoveLeftCommand { get; }
    public IRelayCommand MoveRightCommand { get; }

    public IRelayCommand RestartCommand { get; }



    public GameTwoViewModel(
        IMazeService mazeService,
        IBuffStorage buffStorage,
        ILargeModelService largeModelService)
    {
        _mazeService = mazeService;
        _buffStorage = buffStorage;
        _largeModelService = largeModelService;
        StartNewGame();
        MoveUpCommand = new RelayCommand(MoveUp);
        MoveDownCommand = new RelayCommand(MoveDown);
        MoveLeftCommand = new RelayCommand(MoveLeft);
        MoveRightCommand = new RelayCommand(MoveRight);
        RestartCommand = new RelayCommand(Restart);
        QuitCommand = new RelayCommand(Quit);
        GetPuzzleCommand = new RelayCommand(GetPuzzle);
        ChooseACommand = new RelayCommand(ChooseA);
        ChooseBCommand = new RelayCommand(ChooseB);
        ChooseCCommand = new RelayCommand(ChooseC);
        ChangeCommand = new RelayCommand(changeviable);
        ClosePopCommand = new RelayCommand(close);
    }

    public void GetPuzzle()
    {
        if (theme!=null)
        {
             this.CurrentPuzzle =
                       _largeModelService.GeneratePuzzle(theme);
             changeviable();
        }
        else
        {
            Console.WriteLine("THEME == NULL");
        }
      
    }



    public IRelayCommand ChooseACommand { get; }
    
    public IRelayCommand ChooseBCommand { get; }
    
    public IRelayCommand ChooseCCommand { get; }
    
    
    public void ChooseA()
    {
        this.PlayerAnswer = "A";
        checkAnswer();
    }
    
    public void ChooseB()
    {
        this.PlayerAnswer = "B";
        checkAnswer();
    }
    
    public void ChooseC()
    {
        this.PlayerAnswer = "C";
        checkAnswer();
    }

    public async Task<bool> checkAnswer()
    {
        Console.WriteLine(this.CurrentPuzzle.CorrectAnswer);
        Console.WriteLine(this.PlayerAnswer.Equals(this.CurrentPuzzle.CorrectAnswer));
        if (this.PlayerAnswer.Equals(this.CurrentPuzzle.CorrectAnswer))
        {
            Reply = "回答正确 属性得到进一步加强！！\n"+CurrentPuzzle.explanation;
           Buff newBuff = new Buff
            {
                Name = "火元素",
                Description = "火元素之力: 攻击力+10%。",
                Value1 = 10, // 攻击力加成
                Value2 = 0, // 无生命力加成
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            if (newBuff != null)
            {
                var existingBuff = await _buffStorage.GetBuffAsync(b => b.Name == newBuff.Name);
                if (existingBuff != null)
                {
                    // 更新已有 Buff 的数值
                    existingBuff.Value1 += newBuff.Value1;
                    existingBuff.Value2 += newBuff.Value2;
                    existingBuff.UpdatedAt = DateTime.Now;
                    await _buffStorage.SaveBuffsAsync(existingBuff);
                }
                else
                {
                    // 添加新的 Buff
                    await _buffStorage.AddBuffsAsync(newBuff);
                }

            }
        }
        else
        {
            Reply = "回答错误 ！！很遗憾无法获得元素的奖励\n"+CurrentPuzzle.explanation;
           
        }
        
        
        await Task.Delay(5000); // 暂停3秒
        GoToResultView();
        return this.PlayerAnswer.Equals(this.CurrentPuzzle.CorrectAnswer);
    }
    
    
    
    //加载数据库属性
    public async Task LoadPlayerAttributesAsync()
    {
        var buffs = await _buffStorage.GetBuffsAsync(b => true, 0, int.MaxValue); // 获取所有 Buff
        foreach (var buff in buffs)
        {
            if (!string.IsNullOrEmpty(buff.Name))
            {
                // 将属性字符添加到玩家的属性集合中
                foreach (var ch in buff.Name)
                {
                    switch (ch)
                    {
                        case '火':
                            _playerAttributes.Add('F'); // 火属性
                            break;
                        case '冰':
                            _playerAttributes.Add('B'); // 冰属性
                            break;
                        case '雷':
                            _playerAttributes.Add('T'); // 雷属性
                            break;
                        case '风':
                            _playerAttributes.Add('W'); // 风属性
                            break;
                        case '岩':
                            _playerAttributes.Add('Y'); // 岩属性
                            break;
                    }
                }
            }
        }
    }

    
    //清除属性缓存
    public async Task Delect()
    {
        _playerAttributes.Clear();
    }
    
    public void StartNewGame()
    {
        _mazeService.GenerateMaze();
        LoadPlayerAttributesAsync();
        Console.WriteLine("buff:");
        foreach (var attribute in _playerAttributes)
        {
            Console.WriteLine(attribute);
        }
        _musicPlayer.PlayBackgroundMusic("Assets/BackGround_2.wav"); // 播放背景音乐
        _playerX = 1; // 玩家初始位置
        _playerY = 1;
        GameStatus = "游戏进行中...";
        UpdateMaze();
    }


    public bool CanPassWall(char wallType)
    {
        if (wallType == ' '||wallType =='1'||wallType =='2'||wallType =='3'||wallType =='4') return true; // 普通墙或空地
        if (wallType == '#') return false;
       
        return _playerAttributes.Contains(wallType); // 检查玩家是否拥有该属性
    }

    public void UpdateMaze()
    {

        // 获取迷宫的表示
        var mazeRepresentation = _mazeService.GetMazeRepresentation();

        if (string.IsNullOrEmpty(mazeRepresentation))
        {
            GameStatus = "迷宫加载失败！";
            return;
        }

        var mazeArray = mazeRepresentation.Split('\n');
        MazeGrid.Clear();

        for (int i = 0; i < RowCount; i++)
        {
            var mazeRow = new ObservableCollection<MazeCell>();
            for (int j = 0; j < ColumnCount; j++)
            {
                char character = mazeArray[i][j];
                var mazeCell = new MazeCell
                {
                    Character = character.ToString(),
                    Image = GetImageForCharacter(character),
                    Row = i,
                    Column = j
                };

                // 如果是玩家的位置，更新玩家图像
                if (_playerX == j && _playerY == i)
                {
                    mazeCell.Image = ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/Player.png"));
                }

                mazeRow.Add(mazeCell);
            }

            MazeGrid.Add(mazeRow);
        }

    }

    private Bitmap GetImageForCharacter(char character)
    {
        // 根据字符返回对应的图片
        switch (character)
        {
            case 'P':
                return ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/Player.png"));
            case '#':
                return ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/Wall.png"));
            case ' ':
                return ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/Floor.png"));
            case 'F':
                return ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/FireWall.png"));
            case 'B':
                return ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/SeaWall.png"));
            case 'W':
                return ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/WindWall.png"));
            case 'T':
                return ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/ThunderWall.png"));
            case 'Y':
                return ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/RockWall.png"));
            case '1':
                return ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/火元素.png"));
            case '2':
                return ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/雷元素.png"));
            case '3':
                return ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/风元素.png"));
            case '4':
                return ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/冰元素.png"));
            
            
            
            
            default:
                return ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/Floor.png"));
        }
    }

    // 玩家移动
    public void MoveUp()
    {
        _musicPlayer.PlaySoundEffect("Assets/Move.wav"); // 播放移动音效
        MakeMove(0, -1);
    }

    public void MoveDown()
    {
        _musicPlayer.PlaySoundEffect("Assets/Move.wav"); // 播放移动音效
        MakeMove(0, 1);
    }

    public void MoveLeft()
    {
        _musicPlayer.PlaySoundEffect("Assets/Move.wav"); // 播放移动音效
        MakeMove(-1, 0);
    }

    public void MoveRight()
    {
        _musicPlayer.PlaySoundEffect("Assets/Move.wav"); // 播放移动音效
        MakeMove(1, 0);
    }

    private void MakeMove(int dx, int dy)
    {
        Console.WriteLine("currentbuff:");
        foreach (var attribute in _playerAttributes)
        {
            Console.WriteLine(attribute);
        }
        int newX = _playerX + dx;
        int newY = _playerY + dy;

        // 检查是否越界
        if (newX < 0 || newY < 0 || newX >= 15 || newY >= 15) return;

        char targetWall = _mazeService.GetMazeRepresentation().Split('\n')[newY][newX];

        // 检查墙类型
        if (!CanPassWall(targetWall))
        {
            switch (targetWall)
            {
                case 'F' :
                    GameStatus = "无法通过火焰 需要拥有强大的火元素";
                    break;
                case 'W' :
                    GameStatus = "无法通过飓风 需要拥有强大的风元素";
                    break;
                case 'Y' :
                    GameStatus = "无法通过高大的岩石 需要强大的岩元素";
                    break;
                case 'B' :
                    GameStatus = "无法通过极寒之地 需要强大的冰元素";
                    break;
                case 'T' :
                    GameStatus = "无法通过雷电区域 需要强大的雷元素驾驭雷电";
                    break;
                case '#' :
                    GameStatus = "墙壁不可通过";
                    break;
                
            }
            return;
        }

        // 更新玩家位置
        _playerX = newX;
        _playerY = newY;
        UpdateMaze();

        // 检查终点
        // 创建一个字典，映射玩家的坐标到通关信息
        var exitMessages = new Dictionary<(int, int), string>
        {
            {(1, 14), "恭喜你获得强大的火元素 攻击力+10%"},
            {(13, 14),"恭喜你获得强大的雷元素 防御力+10%"},
            {(14, 1), "恭喜你获得强大的风元素 生命值力+10%"},
            {(14, 5), "恭喜你获得强大的冰元素 攻击力+10%"}
        };

// 检查玩家位置是否在出口列表中
        if (exitMessages.ContainsKey((_playerX, _playerY)))
        {
            WinStatus = exitMessages[(_playerX, _playerY)];
        
            GameStatus = "游戏成功 元素的谜题正在从遥远的提瓦特大陆赶来  稍等片刻";
            Task.Delay(3000);
            isOK = true;
            AddBuffForExit(_playerX, _playerY);
            GetPuzzle();
            StartNewGame();
        }

    }


    /// <summary>
    /// 根据不同的出口为玩家增加 Buff
    /// </summary>
    private async void AddBuffForExit(int x, int y)
    {
        Buff newBuff = null;

        if (x == 1 && y == 14) // 出口 1：火元素 Buff
        {
            theme = "火";
            newBuff = new Buff
            {
                Name = "火元素",
                Description = "火元素之力: 攻击力+10%。",
                Value1 = 10, // 攻击力加成
                Value2 = 0, // 无生命力加成
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
        }
        else if (x == 13 && y == 14) // 出口 2：雷元素 Buff
        {
            theme = "雷";
            newBuff = new Buff
            {
                Name = "雷元素",
                Description = "雷元素之力: 生命力+10%。",
                Value1 = 0, // 无攻击力加成
                Value2 = 10, // 生命力加成
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
        }
        else if (x == 14 && y == 1) // 出口 3：风元素 Buff
        {
            theme = "风";
            newBuff = new Buff
            {
                Name = "风元素",
                Description = "风元素之力: 生命力+10%。",
                Value1 = 0, // 无攻击力加成
                Value2 = 10, // 生命力加成
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
        }
        else if (x == 14 && y == 5) // 出口 4：冰元素 Buff
        {
            theme = "冰";
            newBuff = new Buff
            {
                Name = "冰元素",
                Description = "冰元素之力: 生命力+10%。",
                Value1 = 0, // 无攻击力加成
                Value2 = 10, // 生命力加成
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
        }
        
       
        // 检查是否已经存在 Buff，避免重复添加
        if (newBuff != null)
        {
            var existingBuff = await _buffStorage.GetBuffAsync(b => b.Name == newBuff.Name);
            if (existingBuff != null)
            {
                // 更新已有 Buff 的数值
                existingBuff.Value1 += newBuff.Value1;
                existingBuff.Value2 += newBuff.Value2;
                existingBuff.UpdatedAt = DateTime.Now;
                await _buffStorage.SaveBuffsAsync(existingBuff);
            }
            else
            {
                // 添加新的 Buff
                await _buffStorage.AddBuffsAsync(newBuff);
            }

        }
        
    }



    public IRelayCommand QuitCommand { get; }
    
    private IRelayCommand GetPuzzleCommand { get; }

    private void Quit()
    {
        this.isOK = false;
        Delect();
        LoadPlayerAttributesAsync();
        GoToResultView();
        StartNewGame();
    }

    private void Restart()
    {
        this.isOK = false;
        Delect();
        LoadPlayerAttributesAsync();
        GameStatus = "游戏进行中...";
        _playerX = 1;
        _playerY = 1;
        UpdateMaze();
    }


    public void GoToResultView()
    {
        try
        {
            // 仅在播放器已初始化时停止背景音乐
            if (_musicPlayer != null)
            {
               
            }
            // 发送导航事件
            WeakReferenceMessenger.Default.Send<object, string>(2, "OnForwardNavigation");
            // 根据 isOK 状态处理结果提交
            if (isOK)
            {
                Restart(); // 假设此方法会重启游戏
                WeakReferenceMessenger.Default.Send<object, string>(true, "OnResultSubmitted");
            }
            else
            {
                WeakReferenceMessenger.Default.Send<object, string>(false, "OnResultSubmitted");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GoToResultView 发生错误: {ex.Message}");
        }
    }
}

public class MazeCell
  {
      public string Character { get; set; }  // 表示字符
      public Bitmap Image { get; set; }  // 对应的图片
      public int Row { get; set; }  // 行位置
      public int Column { get; set; }  // 列位置
  }
public class GameStatusMessage
{
    public string Message { get; set; }
}

public class WinStatusMessage
{
    public string Message { get; set; }
}


   
   