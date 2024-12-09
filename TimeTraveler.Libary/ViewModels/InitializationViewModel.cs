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

    public InitializationViewModel(
        IBuffStorage buffStorage,
        IRootNavigationService rootNavigationService
    )
    {
        _buffStorage = buffStorage;
        _rootNavigationService = rootNavigationService;

        OnInitializedCommand = new AsyncRelayCommand(OnInitializedAsync);
    }

    private ICommand OnInitializedCommand { get; }

    public async Task OnInitializedAsync()
    {
        IsInitialized = true;
        if (!_buffStorage.IsInitialized)
        {
            await _buffStorage.InitializeAsync();
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
        // 清空游戏中的属性加成的缓存
        await _buffStorage.RemoveAllBuffAsync();

        _rootNavigationService.NavigateTo(RootNavigationConstant.MainView);
    }
}
