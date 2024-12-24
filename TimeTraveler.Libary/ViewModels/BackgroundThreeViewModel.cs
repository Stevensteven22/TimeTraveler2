using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace TimeTraveler.Libary.ViewModels;

public partial class BackgroundThreeViewModel :ViewModelBase
{
    public BackgroundThreeViewModel() { }

    [RelayCommand]
    public void GoToGameThreeView()
    {
        WeakReferenceMessenger.Default.Send<object, string>(1, "OnForwardNavigation");
        
    }
}