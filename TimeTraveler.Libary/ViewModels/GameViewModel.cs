using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TimeTraveler.Libary.Helpers;
using TimeTraveler.Libary.Models;
using TimeTraveler.Libary.Services;
using Notification = Ursa.Controls.Notification;
using WindowNotificationManager = Ursa.Controls.WindowNotificationManager;

namespace TimeTraveler.Libary.ViewModels;

public partial class GameViewModel : ViewModelBase
{
    public WindowNotificationManager? NotificationManager { get; set; }

    public GameViewModel() { }

    [RelayCommand]
    public void GoToResultView(object? parameter)
    {
        WeakReferenceMessenger.Default.Send<object, string>(2, "OnForwardNavigation");
        
        WeakReferenceMessenger.Default.Send<object, string>(parameter, "OnResultSubmitted");
    }
}
