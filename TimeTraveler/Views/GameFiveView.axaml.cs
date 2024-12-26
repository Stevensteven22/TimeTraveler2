using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.LogicalTree;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Linq;
using TimeTraveler.Dialogs;
using TimeTraveler.Libary.Helpers;
using Notification = Ursa.Controls.Notification;
using WindowNotificationManager = Ursa.Controls.WindowNotificationManager;

namespace TimeTraveler.Views;

public partial class GameFiveView : UserControl
{
    public GameFiveView()
    {
        InitializeComponent();
        WeakReferenceMessenger.Default.Register<object, string>(this, "ShowFiveResult", ShowResult);
    }

    void ShowResult(object recipient, object message)
    {
        var fiveViewModel = ServiceLocator.Current.GameFiveViewModel;
        var result = fiveViewModel.Results;
        var resultSelectorDialogViewModel = new ResultSelectorDialogViewModel
        {
            Results = result,
            AskedTitleText = "属性加成",
            PrimaryButtonContent = "确认结果",
            IsSecondaryButtonVisible = false,
            PrimaryButtonCommand = new RelayCommand<ResultSelectorDialogViewModel>(ConfirmResult),
        };
        resultSelectorDialogViewModel.PrimaryButtonCommandParameter = resultSelectorDialogViewModel;
        DialogHelper.ShowDialog<ResultSelectorDialog, ResultSelectorDialogViewModel>(resultSelectorDialogViewModel, false);
    }

    void ConfirmResult(ResultSelectorDialogViewModel? dialogViewModel)
    {
        if (dialogViewModel is null)
        {
            return;
        }
        if (!dialogViewModel.IsExistChecked())
        {
            ShowTip("请选择一个属性加成结果。");
            return;
        }

        var resultModels = dialogViewModel.Results;
        var selectedResult = resultModels.FirstOrDefault(x => x.IsSelected);
        WeakReferenceMessenger.Default.Send<object, string>(true, "OnResultConfirmed");
        //WeakReferenceMessenger.Default.Send<object, string>(new object(), "OnBackHome");
        WeakReferenceMessenger.Default.Send<object, string>(
            $"恭喜你，{selectedResult?.Name}的属性加成成功！",
            "ShowNotification"
        );
        WeakReferenceMessenger.Default.Send<object, string>(2, "OnForwardNavigation");
        dialogViewModel.Close();
    }

    WindowNotificationManager? NotificationManager { get; set; }

    void ShowTip(string message)
    {
        NotificationManager?.Show(
            new Notification("警告", message),
            showIcon: true,
            showClose: true,
            type: NotificationType.Warning
        );
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        var topLevel = TopLevel.GetTopLevel(this);
        NotificationManager = new WindowNotificationManager(topLevel)
        {
            MaxItems = 3,
            Position = NotificationPosition.TopLeft,
        };
    }

    protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromLogicalTree(e);
        NotificationManager?.Uninstall();
    }
}
