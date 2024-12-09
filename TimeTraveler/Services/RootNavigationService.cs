using System;
using TimeTraveler.Libary.Services;

namespace TimeTraveler.Services;

public class RootNavigationService : IRootNavigationService
{
    public void NavigateTo(string view)
    {
        ServiceLocator.Current.MainWindowViewModel.Content = view switch
        {
            RootNavigationConstant.InitializationView => ServiceLocator
                .Current
                .InitializationViewModel,
            RootNavigationConstant.MainView => ServiceLocator.Current.MainViewModel,
            _ => throw new Exception("未知的视图。"),
        };
    }
}
