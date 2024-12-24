using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace TimeTraveler.Libary.ViewModels;

public partial class ResultThreeViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _result;

    [RelayCommand]
    private void GoToReturnThreeView()
    {
        WeakReferenceMessenger.Default.Send<object, string>(3, "OnForwardNavigation");
    }

    public ResultThreeViewModel()
    {
        OnLoadedCommand = new AsyncRelayCommand(OnLoadedAsync);
        WeakReferenceMessenger.Default.Register<object, string>(this, "OnResultSubmitted", (sender, message) =>
        {
            if (message is bool isSucceed && isSucceed)
            {
                
                IsOK = true;
                Result = "艾琳凭借着坚韧的意志和敏捷的动作，终于避开了最后一波障碍，冲破了风的禁锢。当她再次睁开眼时，风暴已经消散，面前的结界也渐渐消失。她感到一股温暖的气流拂过她的脸庞，风神的考验终于结束。";
                WeakReferenceMessenger.Default.Send<object, string>(new object(), "OnGameSucceed");
            }else 
            {
                IsOK = false;
                Result = "时间旅行者似乎没有通过风之试炼...";
                WeakReferenceMessenger.Default.Send<object, string>(new object(), "OnGameFailed");
            }
        });
       
    }

   
    private ICommand OnLoadedCommand { get; }

    [ObservableProperty]
    private bool _isOK;

    public async Task OnLoadedAsync()
    {
        await Task.CompletedTask;
    }
}
