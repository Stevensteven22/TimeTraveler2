using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TimeTraveler.Libary.Services;

namespace TimeTraveler.Libary.ViewModels;

public partial class InitializationViewModel : ViewModelBase
{
    private readonly IBuffStorage _buffStorage;
    private readonly IRootNavigationService _rootNavigationService;
    private readonly IElementalService _elementalService;

    public InitializationViewModel(
        IBuffStorage buffStorage,
        IRootNavigationService rootNavigationService,
        IElementalService elementalService
    )
    {
        _buffStorage = buffStorage;
        _rootNavigationService = rootNavigationService;
        _elementalService = elementalService;

        OnInitializedCommand = new AsyncRelayCommand(OnInitializedAsync);
    }

    private ICommand OnInitializedCommand { get; }

    public async Task OnInitializedAsync()
    {
        IsInitialized = true;
        if (!_buffStorage.IsInitialized)
        {
            //如果数据库不存在，初始化数据库的数据
            await _buffStorage.InitializeAsync();
            await _elementalService.InitializeElementalAsync();
        }

        await Task.Delay(1000);
        IsInitialized = false;
    }

    [ObservableProperty]
    private bool _isInitialized = false;

    [RelayCommand]
    public void Start()
    {
        _rootNavigationService.NavigateTo(RootNavigationConstant.MainView);
    }

    [RelayCommand]
    private async Task Restart()
    {
        // 发送消息通知游戏重新开始，初始化游戏参数的配置信息
        WeakReferenceMessenger.Default.Send(new object(), "OnRestarted");
        // 清空游戏中的属性加成的缓存，重新初始化游戏的属性加成信息
        //await _buffStorage.RemoveAllBuffAsync();
        await _elementalService.InitializeElementalAsync();

        // 重新开始游戏导航到主页面
        _rootNavigationService.NavigateTo(RootNavigationConstant.MainView);
    }
}
