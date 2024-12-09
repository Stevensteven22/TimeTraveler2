using System;
using Avalonia;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Notifications;
using Avalonia.Controls.Primitives;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Messaging;
using TimeTraveler.UserControls;
using Notification = Ursa.Controls.Notification;
using WindowNotificationManager = Ursa.Controls.WindowNotificationManager;

namespace TimeTraveler.Views;

[PseudoClasses(PC_IsNavigated)]
public partial class MainView : UserControl
{
    public const string PC_IsNavigated = ":isNavigationEnabled";
    public bool IsNavigationButtonEnabled
    {
        get => GetValue(IsNavigationButtonEnabledProperty);
        set => SetValue(IsNavigationButtonEnabledProperty, value);
    }

    public static readonly StyledProperty<bool> IsNavigationButtonEnabledProperty =
        AvaloniaProperty.Register<MainView, bool>(
            nameof(IsNavigationButtonEnabled),
            defaultValue: false
        );

    public MainView()
    {
        InitializeComponent();
        IsNavigationButtonEnabledProperty.Changed.AddClassHandler<MainView>(
            IsNavigationButtonEnabledPropertyChanged
        );

        WeakReferenceMessenger.Default.Register<object, string>(this, "ShowNotification", ShowNotification);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        if (this.IsNavigationButtonEnabled)
            this.SetPseudoclasses(PC_IsNavigated, true);
        else
            this?.SetPseudoclasses(PC_IsNavigated, false);
    }

    private static void IsNavigationButtonEnabledPropertyChanged(
        object sender,
        AvaloniaPropertyChangedEventArgs e
    )
    {
        var control = sender as MainView;
        if (control != null && control.IsNavigationButtonEnabled)
            control.SetPseudoclasses(PC_IsNavigated, true);
        else
            control?.SetPseudoclasses(PC_IsNavigated, false);
    }

    private void SetPseudoclasses(string name, bool flag)
    {
        PseudoClasses.Set(name, flag);
    }

    #region 消息通知提示窗

    public WindowNotificationManager? NotificationManager { get; set; }

    public void ShowNotification(object sender, object message)
    {
        Enum.TryParse<NotificationType>("Success", out var notificationType);
        NotificationManager?.Show(
            new Notification("提示", message.ToString()),
            showIcon: true,
            showClose: true,
            type: notificationType
        );
        return;
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        var topLevel = TopLevel.GetTopLevel(this);
        NotificationManager = new WindowNotificationManager(topLevel)
        {
            MaxItems = 3,
            Position = NotificationPosition.TopCenter,
        };
    }

    protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromLogicalTree(e);
        NotificationManager?.Uninstall();
    }

    #endregion
}
