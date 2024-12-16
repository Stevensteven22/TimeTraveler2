using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TimeTraveler.Libary.Definitions;
using TimeTraveler.Libary.Models;
using TimeTraveler.Libary.Services;
using Ursa.Controls;

namespace TimeTraveler.Libary.ViewModels;

public partial class GameThreeViewModel :ViewModelBase
{
        private bool _isGameWon = false; // 游戏是否胜利
        private bool _isGameOver = false; // 游戏是否结束（碰撞或胜利）
        public  Flyer _ball {get; set;}
        public Obstacle _obstacle1 { get; set; }
        private Obstacle _obstacle2;
        private Obstacle _obstacle3;
        private Obstacle _obstacle4;
        private int score = 0;
        private readonly IFlyService _flyService;
        private DispatcherTimer _timer;
        private const double FrameDuration = 16; // 每帧16ms，即60FPS
        private const double SecondsPerFrame = FrameDuration / 1000.0; // 每帧的秒数
        public double _timeElapsedInSeconds = 0.0; // 游戏已进行的时间（秒）
        private const double GameDurationInSeconds = 10.0; // 游戏持续时长（秒）
        private ElementPoint _elementPoint1;
        private ElementPoint _elementPoint2;
        private string _message;
        private readonly IElementalService _elementalService;

        
        
        // 硬编码的窗口宽高
        private const double WindowWidth = 1200;
        private const double WindowHeight = 800;
        
        public bool IsGameWon
        {
            get => _isGameWon;
            set
            {
                if (_isGameWon != value)
                {
                    _isGameWon = value;
                    OnPropertyChanged(nameof(IsGameWon));
                }
            }
        }
        
        public bool IsGameOver
        {
            get => _isGameOver;
            set
            {
                if (_isGameOver != value)
                {
                    Console.WriteLine(score+"score");
                    _isGameOver = value;
                    OnPropertyChanged(nameof(IsGameOver)); // 用于显示/隐藏游戏结束按钮
                }
            }
        }
        
        public string Message
        {
            get => _message;
            set
            {
                if (_message != value)
                {
                    _message = value;
                    OnPropertyChanged(nameof(Message));  // 当消息更新时，通知UI进行更新
                }
            }
        }

        public double ObstacleX1 => _obstacle1.X;
        public double ObstacleY1 => _obstacle1.Y;
        public double ObstacleWidth1 => _obstacle1.Width;
        public double ObstacleHeight1 => _obstacle1.Height;
        

        
        public double ObstacleX2 => _obstacle2.X;
        public double ObstacleY2 => _obstacle2.Y;
        public double ObstacleWidth2 => _obstacle2.Width;
        public double ObstacleHeight2 => _obstacle2.Height;
        

        public double ObstacleX3 => _obstacle3.X;
        public double ObstacleY3 => _obstacle3.Y;
        public double ObstacleWidth3 => _obstacle3.Width;
        public double ObstacleHeight3 => _obstacle3.Height;
        
        public double ObstacleX4 => _obstacle4.X;
        public double ObstacleY4 => _obstacle4.Y;
        public double ObstacleWidth4 => _obstacle4.Width;
        public double ObstacleHeight4 => _obstacle4.Height;
        
        public double ElementPointX1 => _elementPoint1.X;
        public double ElementPointY1 => _elementPoint1.Y;
        public double ElementPointWidth1 => _elementPoint1.Width;
        public double ElementPointHeight1 => _elementPoint1.Height;
        
        public double ElementPointX2 => _elementPoint2.X;
        public double ElementPointY2 => _elementPoint2.Y;
        public double ElementPointWidth2 => _elementPoint2.Width;
        public double ElementPointHeight2 => _elementPoint2.Height;

        public double BallX => _ball.X;
        public double BallY => _ball.Y;
        public double BallWidth => _ball.Width;
        public double BallHeight => _ball.Height;
        
        public double Score => score;
        
        // 重置游戏状态
        public void ResetGame()
        {
            _ball = new Flyer(100, 100, 100, 100); // 初始化小球
            _obstacle1 = new Obstacle(1200, 500, 100, 300);
            _obstacle2 = new Obstacle(1200, 600, 100, 200);
            _obstacle3 = new Obstacle(1200, 650, 150, 150);
            _obstacle4 = new Obstacle(1200, 0, 1000, 42);
            _elementPoint1 = new ElementPoint(1250, 350, 50, 50);
            _elementPoint2 = new ElementPoint(1250, 450, 50, 50);
            
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(16) // 60FPS
            };
            _timer.Tick += (sender, e) => TimerTick();
            _timer.Start();

            _isGameWon = false;
            _isGameOver = false;
            _timeElapsedInSeconds = 0;
            score = 0;
            IsGameOver = false;  // 通过命令将 IsGameOver 设置为 false，隐藏 Next 按钮
            OnPropertyChanged(nameof(IsGameOver));  // 通知 UI 更新
            IsGameWon = false;
            OnPropertyChanged(nameof(IsGameWon));
        }
        

        
        public ICommand FlapCommand { get; }
        public ICommand RestartCommand { get; }
        
        public GameThreeViewModel(IFlyService flyService,IElementalService elementalService)
        {
            _flyService = flyService;
            _elementalService = elementalService;

                _ball = new Flyer(100, 100, 100, 100); // 初始化小球
                _obstacle1 = new Obstacle(1200, 500, 100, 300);
                _obstacle2 = new Obstacle(1200, 600, 100, 200);
                _obstacle3 = new Obstacle(1200, 650, 150, 150);
                _obstacle4 = new Obstacle(1200, 0, 1000, 42);
                _elementPoint1 = new ElementPoint(1250, 350, 50, 50);
                _elementPoint2 = new ElementPoint(1250, 450, 50, 50);

                _timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(16) // 60FPS
                };
                _timer.Tick += (sender, e) => TimerTick();
                _timer.Start();

                _isGameWon = false;
                _isGameOver = false;
                _timeElapsedInSeconds = 0;
            

        // 使用 RelayCommand 实现 FlapCommand
            FlapCommand = new RelayCommand(Flap);
            RestartCommand = new RelayCommand(ResetGame);
        }

        public void DestoryElement(ElementPoint elementPoint)
        {
            if (elementPoint == _elementPoint1)
            {
                _elementPoint1.IsPickedUp = true;
                _elementPoint1.X = -1000; // 将其移出屏幕
            }
            
            if (elementPoint == _elementPoint2)
            {
                _elementPoint2.IsPickedUp = true;
                _elementPoint2.X = -1000; // 将其移出屏幕
            }
      
        }
        
        
        public void Update()
        {
            if (_isGameWon || _isGameOver)
            {
                _ball.Velocity = 0;
                return; // 游戏结束，停止更新
            }
            _ball.UpdatePosition();
            OnPropertyChanged(nameof(BallX));  
            OnPropertyChanged(nameof(BallY));  
            
            _obstacle1.UpdatePosition3(3); 
            OnPropertyChanged(nameof(ObstacleX1)); 
            OnPropertyChanged(nameof(ObstacleY1)); 
            
            _elementPoint1.UpdatePosition3(3); 
            OnPropertyChanged(nameof(ElementPointX1));
            OnPropertyChanged(nameof(ElementPointY1));

            _obstacle2.UpdatePosition1(5);  
            OnPropertyChanged(nameof(ObstacleX2)); 
            OnPropertyChanged(nameof(ObstacleY2)); 
            
            _elementPoint2.UpdatePosition1(5); 
            OnPropertyChanged(nameof(ElementPointX2));
            OnPropertyChanged(nameof(ElementPointY2));
            
            _obstacle3.UpdatePosition1(6);  
            OnPropertyChanged(nameof(ObstacleX3)); 
            OnPropertyChanged(nameof(ObstacleY3)); 
            
            _obstacle4.UpdatePosition2(10);  
            OnPropertyChanged(nameof(ObstacleX4)); 
            OnPropertyChanged(nameof(ObstacleY4)); 

           //碰撞检测
           if (CheckCollision())
           {
                 GameOver();
                 return;
            }
           
            // 限制小球在屏幕内跳跃，防止超出边界
           if (_ball.Y + _ball.Height > WindowHeight) // 检查是否触地
           {
               _ball.Y = WindowHeight - _ball.Height; // 限制在地面之上
               _ball.Velocity = 0; // 停止下落
           }

           if (_ball.Y < 0) // 检查是否跳出顶部
           {
               _ball.Y = 0; // 限制在顶部
               _ball.Velocity = 0; // 停止上升
           }
        }
        
        // 碰撞检测逻辑
        public bool CheckCollision()
        {
            var obstacles = new[] { _obstacle1, _obstacle2, _obstacle3, _obstacle4 };
            var elementpoints = new[] { _elementPoint1 ,_elementPoint2};
            
            foreach (var obstacle in obstacles)
            {
                // 检查障碍物是否已经超出屏幕范围，避免不必要的计算
                if (obstacle.X + obstacle.Width < 0) // 如果障碍物已经完全移出屏幕
                {
                    continue; 
                }
                // 检查小球是否与障碍物的水平边界重叠
                bool isBallInHorizontalRange = _ball.X + _ball.Width > obstacle.X && _ball.X < obstacle.X + obstacle.Width;

                // 检查小球是否与障碍物的垂直边界重叠
                bool isBallInVerticalRange = _ball.Y + _ball.Height > obstacle.Y && _ball.Y < obstacle.Y + obstacle.Height;

                // 如果小球在水平和垂直范围内，则发生碰撞
                if (isBallInHorizontalRange && isBallInVerticalRange)
                {
                    return true; 
                }
            }
            
            foreach (var elementpoint in elementpoints)
            {
                // 检查障碍物是否已经超出屏幕范围，避免不必要的计算
                if (elementpoint.X + elementpoint.Width < 0) // 如果障碍物已经完全移出屏幕
                {
                    continue; 
                }
                bool isBallInHorizontalRange = _ball.X + _ball.Width > elementpoint.X && _ball.X < elementpoint.X + elementpoint.Width;
                bool isBallInVerticalRange = _ball.Y + _ball.Height > elementpoint.Y && _ball.Y < elementpoint.Y + elementpoint.Height;
                if (isBallInHorizontalRange && isBallInVerticalRange)
                {
                    score += 1; // 得分加一
                    DestoryElement(elementpoint);
                }
            }
            return false; 
        }
        
      
        // 游戏结束处理
        public async Task GameOver()
        {
            IsGameOver = true; // 设置游戏结束
            _timer.Stop(); // 停止游戏更新
            _ball.Velocity = 0; // 停止小球运动
            
        }
        
        public async void GameWon()
        {
            IsGameWon = true; // 设置游戏胜利
            _timer.Stop(); // 停止游戏更新
            SetSuccessMessage();
            _ball.Velocity = 0; // 停止小球运动
            
                // 构建查询表达式
                Expression<Func<ResultModel, bool>> predicate = model => model.Name == "风元素";
                // 查询当前的风元素数据
                ResultModel windElement = await _elementalService.GetElementalAsync(predicate);
                if (windElement != null)
                {
                    // 更新风元素的值
                    windElement.IsSelected = true;
                    windElement.ImprovedValue1 += score;
                    var updatedCollection = new ObservableCollection<ResultModel> { windElement };
                    await _elementalService.InsertOrUpdateElementalAsync(updatedCollection);
                }
                else
                {
                    // 如果不存在风元素数据，插入新数据
                    var newElement = new ResultModel()
                    {
                        Id = 2,
                        Name = "风元素",
                        ResultElementType = ElementType.WindElemental,
                        ImprovedValue1 = score,
                        IsSelected = true
                    };
                    var newCollection = new ObservableCollection<ResultModel> { newElement };
                    await _elementalService.InsertOrUpdateElementalAsync(newCollection);
                }
            

        }

        public void SetSuccessMessage()
        {
            Message = $"试炼成功！ 闪避率提升{score}%";  
            IsGameWon = true;        // 更新游戏状态
        }


        // 小球跳跃的逻辑
        public void Flap()
        {
            if (_isGameWon || _isGameOver) return; // 游戏结束时，禁用跳跃
            _ball.Flap();
            OnPropertyChanged(nameof(BallY));  // 触发跳跃时更新Y位置
        }

        public void TimerTick()
        {
            if (_isGameWon || _isGameOver)
            {
                return; 
            }
            
            _timeElapsedInSeconds += SecondsPerFrame;
            
            if (_timeElapsedInSeconds >= GameDurationInSeconds)
            {
                if (!CheckCollision()) 
                {
                    GameWon(); 
                    
                }
            }
            Update();
        }
        

        
        [RelayCommand]
        public void GoToResultThreeView()
        {
            WeakReferenceMessenger.Default.Send<object, string>(2, "OnForwardNavigation");
            
            if (_isGameWon)
            {
                WeakReferenceMessenger.Default.Send<object, string>(true, "OnResultSubmitted");
            }
            else
            {
                WeakReferenceMessenger.Default.Send<object, string>(false, "OnResultSubmitted");
            }
            
        }
}