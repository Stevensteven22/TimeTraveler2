using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Notifications;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Messaging;
using TimeTraveler.Libary.Definitions;
using Notification = Ursa.Controls.Notification;
using WindowNotificationManager = Ursa.Controls.WindowNotificationManager;

namespace TimeTraveler.UserControls;

[PseudoClasses(":isAnimationEnabled", ":isClicked")]
public class Door : TemplatedControl
{
    public object SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public static readonly StyledProperty<object> SelectedItemProperty = AvaloniaProperty.Register<
        Door,
        object
    >(nameof(SelectedItem));

    public ElementType DoorElementType
    {
        get => GetValue(DoorElementTypeProperty);
        set => SetValue(DoorElementTypeProperty, value);
    }

    public static readonly StyledProperty<ElementType> DoorElementTypeProperty =
        AvaloniaProperty.Register<Door, ElementType>(nameof(DoorElementType));

    public ICommand ClickCommand
    {
        get => GetValue(ClickCommandProperty);
        set => SetValue(ClickCommandProperty, value);
    }

    public static readonly StyledProperty<object> ClickCommandParameterProperty =
        AvaloniaProperty.Register<Door, object>(nameof(ClickCommandParameter));

    public object ClickCommandParameter
    {
        get => GetValue(ClickCommandParameterProperty);
        set => SetValue(ClickCommandParameterProperty, value);
    }

    public static readonly StyledProperty<ICommand> ClickCommandProperty =
        AvaloniaProperty.Register<Door, ICommand>(nameof(ClickCommand));

    public static readonly RoutedEvent<RoutedEventArgs> ClickedEvent = RoutedEvent.Register<
        Door,
        RoutedEventArgs
    >(nameof(Clicked), RoutingStrategies.Direct);

    public event EventHandler<RoutedEventArgs> Clicked
    {
        add => AddHandler(ClickedEvent, value);
        remove => RemoveHandler(ClickedEvent, value);
    }

    protected virtual void OnClicked()
    {
        RoutedEventArgs args = new RoutedEventArgs(ClickedEvent);
        RaiseEvent(args);
    }

    public Button PART_ContentControl =>
        this.GetTemplateChildren().FirstOrDefault(e => e.Name == "PART_ContentControl") as Button;

    public ListBox PART_AnimationList =>
        this.GetTemplateChildren().FirstOrDefault(e => e.Name == "PART_AnimationList") as ListBox;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        //PART_AnimationList.Loaded += PART_AnimationList_OnLoaded;
        SetPseudoclasses("isAnimationEnabled", true);
        PART_AnimationList.SelectedIndex = 0;
        OpenDoorCompleted += () =>
        {
            OnClicked();
            ClickCommand?.Execute(ClickCommandParameter);
        };
        PART_ContentControl.Click += async (sender, args) =>
        {
            await AnimateToOpenDoor();
        };

        WeakReferenceMessenger.Default.Register<object, string>(
            this,
            "OnRestarted",
            (r, p) =>
            {
                OnRestarted();
            }
        );
    }

    private void OnRestarted()
    {
        InitializeGameOfDoor();
    }

    private void InitializeGameOfDoor()
    {
        //初始画门动画
        PART_AnimationList.SelectedIndex = 0;
        //初始化点击时候的伪类
        if (PseudoClasses.Contains(":isClicked"))
            SetPseudoclasses("isClicked", false);
    }

    private Action OpenDoorCompleted;

    private DispatcherTimer _timer;
    int selectedIndex = 0;

    private void PART_AnimationList_OnLoaded(object? sender, RoutedEventArgs e)
    {
        var listBox = sender as ListBox;
        // 初始化定时器
        _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(150) };
        _timer.Tick += (s, e) =>
        {
            listBox.SelectedIndex = selectedIndex;
            selectedIndex++;
            if (selectedIndex == 5)
                selectedIndex = 0;
        };
        _timer.Start();
    }

    #region 消息通知提示窗

    public WindowNotificationManager? NotificationManager { get; set; }

    public void ShowTip(string message)
    {
        Enum.TryParse<NotificationType>("Warning", out var notificationType);
        NotificationManager?.Show(
            new Notification("警告", message),
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
            Position = NotificationPosition.TopLeft,
        };
    }

    protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromLogicalTree(e);
        NotificationManager?.Uninstall();
    }

    #endregion


    private bool CheckIsClicked()
    {
        if (PseudoClasses.Contains(":isClicked"))
        {
            ShowTip("关卡已通关，不能重复闯关。");
            return true;
        }
        else
        {
            return false;
        }
    }

    private async Task<bool> AnimateToOpenDoor()
    {
        if (CheckIsClicked())
            return false;

        SetPseudoclasses("isClicked", true);

        return await Task.Run(async () =>
            {
                int selectedIndex = 0;
                for (
                    selectedIndex = 0;
                    selectedIndex < PART_AnimationList.Items.Count;
                    selectedIndex++
                )
                {
                    await Task.Delay(200);
                    Dispatcher.UIThread.Invoke(() =>
                    {
                        PART_AnimationList.SelectedIndex = selectedIndex;
                    });
                }

                return true;
            })
            .ContinueWith(t =>
            {
                if (t.IsCompleted)
                {
                    Dispatcher.UIThread.Invoke(() =>
                    {
                        //防止重复可打开门,设置门已经打开过了，不能重复打开
                        //SetPseudoclasses("isClicked", false);
                        OpenDoorCompleted?.Invoke();
                    });
                }

                return t.Result;
            });
    }

    private void SetPseudoclasses(string name, bool flag)
    {
        PseudoClasses.Set($":{name}", flag);
    }
}
