using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TimeTraveler.Libary.Helpers;
using TimeTraveler.Libary.Models;
using TimeTraveler.Libary.Services;

namespace TimeTraveler.Libary.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly IChapterNavigationService _chatperNavigationService;
    private readonly IRootNavigationService _rootNavigationService;

    public MainViewModel(
        IChapterNavigationService chatperNavigationService,
        IRootNavigationService rootNavigationService
    )
    {
        _chatperNavigationService = chatperNavigationService;
        _rootNavigationService = rootNavigationService;
        OnMenuTappedCommand = new RelayCommand<ChapterViewModel>(OnMenuTapped);

        WeakReferenceMessenger.Default.Register<object, string>(
            this,
            "OnForwardNavigation",
            (r, p) =>
            {
                OnForwardNavigation((int)p);
            }
        );

        WeakReferenceMessenger.Default.Register<object, string>(
            this,
            "OnBackHome",
            (r, p) =>
            {
                OnBackHome();
            }
        );

        WeakReferenceMessenger.Default.Register<object, string>(
            this,
            "OnGameSucceed",
            (r, p) =>
            {
                OnGameSucceed();
            }
        );

        WeakReferenceMessenger.Default.Register<object, string>(
            this,
            "OnGameFailed",
            (r, p) =>
            {
                OnGameFailed();
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

        InitializeMenuItemsData();

        // 初始化定时器
        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
        _timer.Tick += Timer_Tick;
        _timer.Start();

        OnSelectionChangedCommand = new RelayCommand(OnSelectionChanged);
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        if (_currentIndex < Images.Count - 1)
        {
            _currentIndex++;
        }
        else
        {
            _currentIndex = 0;
        }
        SelectedImage = Images[_currentIndex];
    }

    private bool _isNavigationEnabled = false;

    public bool IsNavigationEnabled
    {
        get => _isNavigationEnabled;
        set => SetProperty(ref _isNavigationEnabled, value);
    }

    private ViewModelBase _content;

    public ViewModelBase Content
    {
        get => _content;
        set => SetProperty(ref _content, value);
    }

    [ObservableProperty]
    private Bitmap _selectedImage;

    private int _currentIndex;
    private DispatcherTimer _timer;

    public ObservableCollection<Bitmap> Images { get; } =
        new ObservableCollection<Bitmap>()
        {
            ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/原神player1.png")),
            ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/原神player2.png")),
            ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/原神player3.png")),
            ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/原神player4.png")),
            ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/原神player5.png")),
        };
    public ObservableCollection<ChapterViewModel> MenuItems { get; } = new ObservableCollection<ChapterViewModel>();

    private ChapterViewModel _selectedMenuItem;
    public ICommand OnMenuTappedCommand { get; }

    public ICommand OnSelectionChangedCommand { get; }

    public void OnMenuTapped(ChapterViewModel selectedMenuItem)
    {
        _selectedMenuItem = selectedMenuItem;
        IsNavigationEnabled = true;
        var views = ChapterNavigationConstant.ChapterViews[selectedMenuItem.Id];
        _chatperNavigationService.NavigateTo(views[0]);
    }

    public void OnForwardNavigation(int pageIndex)
    {
        if (IsNavigationEnabled && _selectedMenuItem != null)
        {
            var views = ChapterNavigationConstant.ChapterViews[_selectedMenuItem.Id];
            _chatperNavigationService.NavigateTo(views[pageIndex]);
        }
    }

    public void OnBackHome()
    {
        if (IsNavigationEnabled && _selectedMenuItem != null)
        {
            IsNavigationEnabled = false;
            _selectedMenuItem = null;
            _rootNavigationService.NavigateTo(RootNavigationConstant.MainView);
        }
    }

    public void OnGameSucceed()
    {
        if (IsNavigationEnabled && _selectedMenuItem != null)
        {
            var menuItem = MenuItems.FirstOrDefault(x => x.Id == _selectedMenuItem.Id);
            if (menuItem != null)
            {
                menuItem.IsOK = true;
                menuItem.UpdatedTime = DateTime.Now;
            }
        }
    }

    public void OnGameFailed()
    {
        if (IsNavigationEnabled && _selectedMenuItem != null)
        {
            var menuItem = MenuItems.FirstOrDefault(x => x.Id == _selectedMenuItem.Id);
            if (menuItem != null)
            {
                menuItem.IsOK = false;
                menuItem.UpdatedTime = DateTime.Now;
            }
        }
    }

    private void OnRestarted()
    {
        InitializeMenuItemsData();
    }

    private void InitializeMenuItemsData()
    {
        MenuItems.Clear();
        MenuItems.Add(
            new ChapterViewModel
            {
                Id = 1,
                Name = "第一关",
                IsOK = false,
                UpdatedTime = DateTime.Now.AddDays(-6),
            }
        );
        MenuItems.Add(
            new ChapterViewModel
            {
                Id = 2,
                Name = "第二关",
                IsOK = false,
                UpdatedTime = DateTime.Now.AddDays(-5),
            }
        );
        MenuItems.Add(
            new ChapterViewModel
            {
                Id = 3,
                Name = "第三关",
                IsOK = false,
                UpdatedTime = DateTime.Now.AddDays(-4),
            }
        );
        MenuItems.Add(
            new ChapterViewModel
            {
                Id = 4,
                Name = "第四关",
                IsOK = false,
                UpdatedTime = DateTime.Now.AddDays(-3),
            }
        );
        MenuItems.Add(
            new ChapterViewModel
            {
                Id = 5,
                Name = "第五关",
                IsOK = false,
                UpdatedTime = DateTime.Now.AddDays(-2),
            }
        );
        MenuItems.Add(
            new ChapterViewModel
            {
                Id = 6,
                Name = "第六关",
                IsOK = false,
                UpdatedTime = DateTime.Now.AddDays(-1),
            }
        );
        _chatperNavigationService.InitializeMenuItemsQueueToNavigable(MenuItems);
    }

    [RelayCommand]
    private void ReturnToStart()
    {
        _rootNavigationService.NavigateTo(RootNavigationConstant.InitializationView);
    }

    private void OnSelectionChanged() { }
}
