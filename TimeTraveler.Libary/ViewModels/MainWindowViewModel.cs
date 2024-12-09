using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using TimeTraveler.Libary.Services;

namespace TimeTraveler.Libary.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly IRootNavigationService _rootNavigationService;

    public MainWindowViewModel(IRootNavigationService rootNavigationService)
    {
        _rootNavigationService = rootNavigationService;

        OnInitializedCommand = new RelayCommand(OnInitialized);
    }

    private ViewModelBase _content;

    public ViewModelBase Content
    {
        get => _content;
        set => SetProperty(ref _content, value);
    }

    public ICommand OnInitializedCommand { get; }

    public void OnInitialized()
    {
        _rootNavigationService.NavigateTo(RootNavigationConstant.InitializationView);
        //if (!_chapterStorage.IsInitialized ) {
        // _rootNavigationService.NavigateTo(RootNavigationConstant
        //    .InitializationView);
        //} else {
        //_rootNavigationService.NavigateTo(RootNavigationConstant.MainView);
        //}
    }
}
