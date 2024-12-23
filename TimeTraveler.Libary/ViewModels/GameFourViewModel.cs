using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TimeTraveler.Libary.Definitions;
using TimeTraveler.Libary.Helpers;
using TimeTraveler.Libary.Services;
using TimeTraveler.Libary.Models;

namespace TimeTraveler.Libary.ViewModels
{
    public partial class GameFourViewModel : ViewModelBase
    {
        private readonly IResultVerifyFourService _resultVerifyFourService;
        private readonly IElementalService _elementalService;
        
        //当前是第几个选择
        private int _currentorder;
        // 游戏状态
        [ObservableProperty]
        private string _gameStatus;
        //胜利图片展示
        private Boolean _imagesuccess;

        public Boolean Imagesuccess
        {
            get => _imagesuccess;
            set => SetProperty(ref _imagesuccess, value);
        }
        // 用于存储点击顺序的列表
        [ObservableProperty]
        private ObservableCollection<Avatar> _avatars = new ObservableCollection<Avatar>();

        // 当前玩家的点击顺序
        private List<string> _selectedOrder = new List<string>();
        
        // 存储正确的顺序
        private List<string> _correctOrder = new List<string> { "0", "4", "2", "7" }; // 例如：第一行第一个，第二行第二个，第一行第三个，第三行第二个

        // 需要显示的图案
        [ObservableProperty]
        private Bitmap _resultImage=ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/SuccessImage.png"));
        
        //9个按钮按钮颜色
        private string[] _buttoncolors= { "", "", "", "","","","","","" };
        public string Button1Color
        {
            get => _buttoncolors[0];
            set => SetProperty(ref _buttoncolors[0], value);  
        }
        public string Button2Color
        {
            get => _buttoncolors[1];
            set => SetProperty(ref _buttoncolors[1], value);  
        }
        public string Button3Color
        {
            get => _buttoncolors[2];
            set => SetProperty(ref _buttoncolors[2], value);  
        }
        public string Button4Color
        {
            get => _buttoncolors[3];
            set => SetProperty(ref _buttoncolors[3], value);  
        }
        public string Button5Color
        {
            get => _buttoncolors[4];
            set => SetProperty(ref _buttoncolors[4], value);  
        }
        public string Button6Color
        {
            get => _buttoncolors[5];
            set => SetProperty(ref _buttoncolors[5], value);  
        }
        public string Button7Color
        {
            get => _buttoncolors[6];
            set => SetProperty(ref _buttoncolors[6], value);  
        }
        public string Button8Color
        {
            get => _buttoncolors[7];
            set => SetProperty(ref _buttoncolors[7], value);  
        }
        public string Button9Color
        {
            get => _buttoncolors[8];
            set => SetProperty(ref _buttoncolors[8], value);  
        }
        
        //操作指令
        public IRelayCommand OnAvatarClickCommand { get; }
        // 构造函数
        public GameFourViewModel(IResultVerifyFourService resultVerifyFourService, IElementalService elementalService)
        {
            
            _elementalService = elementalService;
            Imagesuccess = false;
            _currentorder = 0;
            _resultVerifyFourService = resultVerifyFourService;
            InitializeAvatars();
            GameStatus = "请按照顺序点击头像!";
            OnAvatarClickCommand = new RelayCommand<string>(OnAvatarClick);
        }

        // 初始化人物头像
        private void InitializeAvatars()
        {
            _avatars.Add(new Avatar { Image = ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/avatar1.png")) });
            _avatars.Add(new Avatar { Image = ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/avatar2.png")) });
            _avatars.Add(new Avatar { Image = ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/avatar3.png")) });
            _avatars.Add(new Avatar { Image = ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/avatar4.png")) });
            _avatars.Add(new Avatar { Image = ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/avatar5.png")) });
            _avatars.Add(new Avatar { Image = ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/avatar6.png")) });
            _avatars.Add(new Avatar { Image = ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/avatar7.png")) });
            _avatars.Add(new Avatar { Image = ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/avatar8.png")) });
            _avatars.Add(new Avatar { Image = ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/avatar9.png")) });
        }

        // 处理头像点击事件
        [RelayCommand]
        public void OnAvatarClick(string avatarIndex)
        {
            //检查是否已选择
            if (!_selectedOrder.Contains(avatarIndex))
            {
                _currentorder++;
                // 如果玩家点击的是正确的顺序，则继续
                _selectedOrder.Add(avatarIndex);
                switch (avatarIndex)
                {
                    case "0":
                        Button1Color = GetColorByOrder(_currentorder);
                        break;
                    case "1":
                        Button2Color = GetColorByOrder(_currentorder);
                        break;
                    case "2":
                        Button3Color = GetColorByOrder(_currentorder);
                        break;
                    case "3":
                        Button4Color = GetColorByOrder(_currentorder);
                        break;
                    case "4":
                        Button5Color = GetColorByOrder(_currentorder);
                        break;
                    case "5":
                        Button6Color = GetColorByOrder(_currentorder);
                        break;
                    case "6":
                        Button7Color = GetColorByOrder(_currentorder);
                        break;
                    case "7":
                        Button8Color = GetColorByOrder(_currentorder);
                        break;
                    case "8":
                        Button9Color = GetColorByOrder(_currentorder);
                        break;
                }
                // 判断是否点击完了所有头像
                if (_selectedOrder.Count == _correctOrder.Count)
                {
                    CheckOrder();
                }
            }
        }
        //根据状态返回颜色
        public string GetColorByOrder(int order)
        {
            switch (order)
            {
                case 1:
                    return "LightCoral";
                case 2:
                    return "Yellow";
                case 3:
                    return "SkyBlue";
                case 4:
                    return "LimeGreen";
                default:
                    return "";
            }
        }
        // 检查玩家点击的顺序
        private async void  CheckOrder()
        {
            if (_selectedOrder.SequenceEqual(_correctOrder))
            {
                // 构建查询表达式
                Expression<Func<ResultModel, bool>> predicate = model => model.Name == "风元素";
                // 查询当前的风元素数据
                ResultModel windElement = await _elementalService.GetElementalAsync(predicate);
                if (windElement != null)
                {
                    // 更新风元素的值
                    windElement.IsSelected = true;
                    windElement.ImprovedValue1 += 10;
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
                        ImprovedValue1 = 10,
                        IsSelected = true
                    };
                    var newCollection = new ObservableCollection<ResultModel> { newElement };
                    await _elementalService.InsertOrUpdateElementalAsync(newCollection);
                }
                
                // 如果顺序正确，显示正确的图案
                GameStatus = "恭喜你完成任务！";
                ShowResultImage();
            }
            else
            {
                // 如果顺序错误，提示重新开始
                Restart();
            }
        }
        //重置函数
        public void Restart()
        {
            GameStatus = "顺序错误，请重新开始!";
            _selectedOrder.Clear();
            Button1Color = "Transparent";
            Button2Color = "Transparent";
            Button3Color = "Transparent";
            Button4Color = "Transparent";
            Button5Color = "Transparent";
            Button6Color = "Transparent";
            Button7Color = "Transparent";
            Button8Color = "Transparent";
            Button9Color = "Transparent";
            _currentorder = 0;
        }
        // 显示结果图案
        private void ShowResultImage()
        {
            // 这里加载成功后的图案，例如:
            Imagesuccess = true;
        }

        // 跳转到结果界面
        [RelayCommand]
        public void GoToResultView()
        {
            WeakReferenceMessenger.Default.Send<object, string>(2, "OnForwardNavigation");
            // 这里执行结果验证和跳转
            if (GameStatus.Equals("恭喜你完成任务！"))
            {
                WeakReferenceMessenger.Default.Send<object, string>(true, "OnResultSubmitted");
            }
            else
            {
                WeakReferenceMessenger.Default.Send<object, string>(false, "OnResultSubmitted");
            }
        }
    }

    // 用于存储头像的类
    public class Avatar
    {
        public Bitmap Image { get; set; }
    }
}
