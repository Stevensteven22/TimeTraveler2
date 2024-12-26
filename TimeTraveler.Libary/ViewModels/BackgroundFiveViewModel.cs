using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace TimeTraveler.Libary.ViewModels;

public partial class BackgroundFiveViewModel : ViewModelBase
{
    public BackgroundFiveViewModel() { }

    [RelayCommand]
    public void GoToGameView()
    {
        WeakReferenceMessenger.Default.Send<object, string>(1, "OnForwardNavigation");
    }
}