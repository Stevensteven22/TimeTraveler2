using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TimeTraveler.Libary.Definitions;
using TimeTraveler.Libary.Services;

namespace TimeTraveler.Libary.ViewModels;

public partial class BackgroundViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool _isGameCompleted = false;

    public BackgroundViewModel()
    {
        WeakReferenceMessenger.Default.Register<object, string>(
            this,
            "OnGameCompleted",
            OnGameCompleted
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
        InitializeGameStart();
    }

    private void InitializeGameStart()
    {
        IsGameCompleted = false;
    }

    private void OnGameCompleted(object recipient, object message)
    {
        IsGameCompleted = true;
    }

    [RelayCommand]
    public void GoToGameView(string? args)
    {
        WeakReferenceMessenger.Default.Send<object, string>(1, "OnForwardNavigation");
        Task.Delay(500)
            .ContinueWith(_ =>
            {
                if (args != null)
                {
                    ElementType.TryParse(args, out ElementType elementType);
                    WeakReferenceMessenger.Default.Send<object, string>(
                        elementType,
                        "SetGameElementType"
                    );
                }
            });
    }

    [RelayCommand]
    private void BackToResult(string? args)
    {
        WeakReferenceMessenger.Default.Send<object, string>(new object(), "ShowResult");
    }
}
