using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.VisualTree;
using CommunityToolkit.Mvvm.Messaging;
using TimeTraveler.Libary.ViewModels;
using Tmds.DBus.Protocol;
using Notification = Ursa.Controls.Notification;
using WindowNotificationManager = Ursa.Controls.WindowNotificationManager;

namespace TimeTraveler.Views;

public partial class GameTwoView : UserControl
{
    private readonly GameTwoViewModel _viewModel;
    public GameTwoView()
    {
        _viewModel = ServiceLocator.Current.GameTwoViewModel;
        InitializeComponent();
        WeakReferenceMessenger.Default.Register<GameStatusMessage>(this, (r, message) =>
        {
           // ShowPopup(message.Message);
            Notification(message.Message);
        });
        WeakReferenceMessenger.Default.Register<WinStatusMessage>(this, (r, message) =>
        {
            ShowPopup(message.Message);
           
        });
    }
    
    
    private async void ShowPopup(string message)
    {
        var window = this.FindAncestorOfType<Window>();
        if (window != null)
        {
            var dialog = new Window
            {
                Width = 400,
                Height = 200,
                CanResize = false,
                Background = new SolidColorBrush(Color.Parse("#F8B7D1")), // 粉色背景
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Opacity = 0, // 初始透明度为0
                SystemDecorations = SystemDecorations.None, // 无操作栏
                Content = new Border
                {
                    CornerRadius = new CornerRadius(15),
                    BorderBrush = new SolidColorBrush(Color.Parse("#F8B7D1")), // 边框颜色类似原神风格
                    BorderThickness = new Thickness(3),
                    Background = new SolidColorBrush(Color.Parse("#F8B7D1")), // 背景颜色更深
                    Padding = new Thickness(30),
                    Child = new TextBlock
                    {
                        Text = message,
                        FontSize = 18,
                        Foreground = Brushes.White,
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                        TextWrapping = Avalonia.Media.TextWrapping.Wrap
                    }
                }
            };

            // 使用 ShowDialog 方法
            dialog.ShowDialog(window);

            // 渐入动画
            for (double i = 0; i <= 1; i += 0.1)
            {
                dialog.Opacity = i;
                await Task.Delay(30);
            }

            await Task.Delay(5000); // 等待3秒

            // 渐出动画
            for (double i = 1; i >= 0; i -= 0.1)
            {
                dialog.Opacity = i;
                await Task.Delay(30);
            }

            dialog.Close();
        }
    }


    
    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Up)
        {
            _viewModel.MoveLeftCommand.Execute(null);
            
        }
        else if (e.Key == Key.Left)
        {
            _viewModel.MoveUpCommand.Execute(null);
        }
        else if (e.Key == Key.Down)
        {
            _viewModel.MoveRightCommand.Execute(null);
        }
        else if (e.Key == Key.Right)
        {
            _viewModel.MoveDownCommand.Execute(null);
        }
        else if (e.Key == Key.Escape) // 例如按 Escape 键放弃
        {
            _viewModel.QuitCommand.Execute(null);
        }
        else if (e.Key == Key.R) // 例如按 R 键重新开始
        {
            _viewModel.RestartCommand.Execute(null);
        }
    }
    
    public WindowNotificationManager? NotificationManager { get; set; }

    public void Notification( object message)
    {
        Enum.TryParse<NotificationType>("Information", out var notificationType);
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

    
    private void Button_PointerEntered_1(object? sender, Avalonia.Input.PointerEventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            button.Background = Brushes.AliceBlue;
        }
    }

    private void Button_PointerExited_2(object? sender, Avalonia.Input.PointerEventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            button.Background = new SolidColorBrush(Color.Parse("#E9E5D9"));
        }
    }
}