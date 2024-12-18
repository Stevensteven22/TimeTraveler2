using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TimeTraveler.Libary.Services;

namespace TimeTraveler.Libary.ViewModels;

public partial class BackgroundFourViewModel : ViewModelBase
{
    public BackgroundFourViewModel() { }

    [RelayCommand]
    public void GoToGameView()
    {
        WeakReferenceMessenger.Default.Send<object, string>(1, "OnForwardNavigation");
    }
}