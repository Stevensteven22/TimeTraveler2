using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;

namespace TimeTraveler.UserControls;

public class FlyoutText : TemplatedControl
{
    public FlyoutText()
    {
        this.Unloaded += FlyoutText_Unloaded;
        this.TextFlown += FlyoutText_TextFlown;
    }

    private void FlyoutText_TextFlown(object? sender, RoutedEventArgs e)
    {
        this.Classes.RemoveAll(this.Classes.ToList());
        this.Classes.Add("Flyout");
    }

    private void FlyoutText_Unloaded(object? sender, RoutedEventArgs e)
    {
        this.Classes.RemoveAll(this.Classes.ToList());
    }

    #region 依赖属性

    public static readonly StyledProperty<string> TextContentProperty = AvaloniaProperty.Register<
        FlyoutText,
        string
    >(nameof(TextContent), string.Empty);

    public string TextContent
    {
        get { return GetValue(TextContentProperty); }
        set { SetValue(TextContentProperty, value); }
    }

    #endregion

    #region  事件

    public static readonly RoutedEvent<RoutedEventArgs> TextFlownEvent = RoutedEvent.Register<
        FlyoutText,
        RoutedEventArgs
    >(nameof(TextFlown), RoutingStrategies.Direct);

    public event EventHandler<RoutedEventArgs> TextFlown
    {
        add => AddHandler(TextFlownEvent, value);
        remove => RemoveHandler(TextFlownEvent, value);
    }

    public virtual void OnTextFlown()
    {
        RoutedEventArgs args = new RoutedEventArgs(TextFlownEvent);
        RaiseEvent(args);
    }

    #endregion
}
