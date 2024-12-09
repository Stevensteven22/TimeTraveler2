using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Notifications;
using Avalonia.Data.Core.Plugins;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Messaging;
using TimeTraveler.Libary.Definitions;
using TimeTraveler.Libary.ViewModels;
using TimeTraveler.UserControls;
using WindowNotificationManager = Ursa.Controls.WindowNotificationManager;

namespace TimeTraveler.Views;

public partial class GameView : UserControl
{
    public ElementType GameElementType
    {
        get => GetValue(GameElementTypeProperty);
        set => SetValue(GameElementTypeProperty, value);
    }

    public static readonly StyledProperty<ElementType> GameElementTypeProperty =
        AvaloniaProperty.Register<GameView, ElementType>(
            nameof(GameElementType),
            ElementType.NoneElemental
        );

    private readonly GameViewModel _viewModel;

    public GameView()
    {
        _viewModel = ServiceLocator.Current.GameViewModel;
        InitializeComponent();

        GameElementTypeProperty.Changed.AddClassHandler<GameView>(OnGameElementTypePropertyChanged);

        WeakReferenceMessenger.Default.Register<object, string>(
            this,
            "SetGameElementType",
            async (sender, message) =>
            {
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    this.GameElementType = (ElementType)message;
                });
            }
        );
    }

    private static void OnGameElementTypePropertyChanged(
        GameView sender,
        AvaloniaPropertyChangedEventArgs e
    )
    {
        // 'e' 参数描述了发生的更改。
        if (e.NewValue is ElementType newElementType && e.OldValue is ElementType oldElementType)
        {
            sender.PART_GameControl.Classes.Remove(oldElementType.ToString());
            sender.PART_GameControl.Classes.Add(newElementType.ToString());
        }
    }

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        PART_GameControl.Classes.Remove(GameElementType.ToString());
    }
}
