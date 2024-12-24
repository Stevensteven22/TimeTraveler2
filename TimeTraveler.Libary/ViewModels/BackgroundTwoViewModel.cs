using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TimeTraveler.Libary.Services;

namespace TimeTraveler.Libary.ViewModels;

public partial class BackgroundTwoViewModel : ViewModelBase
{
    public BackgroundTwoViewModel() { }

    [RelayCommand]
    public void GoToGameView()
    {
        WeakReferenceMessenger.Default.Send<object, string>(1, "OnForwardNavigation");
    }
    private void BackToResult(string? args)
    {
        WeakReferenceMessenger.Default.Send<object, string>(new object(), "ShowResult");
    }
}