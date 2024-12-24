using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace TimeTraveler.Libary.ViewModels;

public partial class ReturnThreeViewModel : ViewModelBase
{
    public ReturnThreeViewModel() { }

    [RelayCommand]
    public void GoBackHomeView()
    {
        WeakReferenceMessenger.Default.Send<object, string>(new object(), "OnBackHome");
    }
}