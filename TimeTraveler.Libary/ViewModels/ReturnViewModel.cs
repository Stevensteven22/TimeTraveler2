using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace TimeTraveler.Libary.ViewModels;

public partial class ReturnViewModel:ViewModelBase
{
    public ReturnViewModel() { }

    [RelayCommand]
    public void GoBackHomeView()
    {
        WeakReferenceMessenger.Default.Send<object, string>(new object(), "OnBackHome");
    }
}