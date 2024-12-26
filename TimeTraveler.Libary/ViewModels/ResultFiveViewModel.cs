using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace TimeTraveler.Libary.ViewModels;

public partial class ResultFiveViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _result;

    [RelayCommand]
    private void GoToReturnView()
    {
        WeakReferenceMessenger.Default.Send<object, string>(3, "OnForwardNavigation");
    }

    public ResultFiveViewModel()
    {
        WeakReferenceMessenger.Default.Register<object, string>(this, "OnResultSubmitted", (sender, message) =>
        {
            if (message is bool isSucceed && isSucceed)
            {
                IsOK = true;
                Result = "  **悬念**:";
                WeakReferenceMessenger.Default.Send<object, string>(new object(), "OnGameSucceed");
            }else 
            {
                IsOK = false;
                Result = "时间旅行者似乎无法破解书中的奥秘.......";
                WeakReferenceMessenger.Default.Send<object, string>(new object(), "OnGameFailed");
            }
        });
    }

    [ObservableProperty]
    private bool _isOK;

}
