using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TimeTraveler.Libary.Services;

namespace TimeTraveler.Libary.ViewModels;

public partial class ReturnViewModel : ViewModelBase
{
    private readonly IRootNavigationService _rootNavigationService;

    [ObservableProperty]
    private string _returnText =
        "完成探险，无论成功与否，都要回到故事的起点。时间会带给你力量，让你重新站起来。时间之旅，是你生命的历程，也是你成长的历程。";

    private bool _isGoToStartView = false;

    public ReturnViewModel(IRootNavigationService rootNavigationService)
    {
        _rootNavigationService = rootNavigationService;
        WeakReferenceMessenger.Default.Register<object, string>(
            this,
            "OnReturnViewArrived",
            (sender, message) =>
            {
                //TODO:利用反射解析object类型的message有哪些属性，然后根据属性的值来设置ReturnText的值
                message
                    .GetType()
                    .GetProperties()
                    .ToList()
                    .ForEach(p =>
                    {
                        if (p.GetValue(message) != null)
                        {
                            if (p.Name == "ReturnText")
                            {
                                SetProperty(
                                    ref _returnText,
                                    p.GetValue(message).ToString(),
                                    p.Name
                                );
                            }
                            if (p.Name == "IsGoToStartView")
                            {
                                _isGoToStartView = (bool)p.GetValue(message);
                            }
                        }
                    });
            }
        );
    }

    [RelayCommand]
    private void GoBackHomeView()
    {
        if (_isGoToStartView)
        {
            WeakReferenceMessenger.Default.Send(new object(), "OnBackHome");

            _rootNavigationService.NavigateTo(RootNavigationConstant.InitializationView);
            _isGoToStartView = false;
        }
        else
            WeakReferenceMessenger.Default.Send(new object(), "OnBackHome");
    }
}
