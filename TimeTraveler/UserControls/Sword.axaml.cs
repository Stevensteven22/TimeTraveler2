using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using OpenCvSharp;

namespace TimeTraveler.UserControls;

public class Sword : TemplatedControl
{
    public Sword()
    {
        this.LeftAttacked += SwordOnLeftAttacked;

        this.RightAttacked += SwordOnRightAttacked;

        this.Unloaded += SwordOnUnLoaded;
    }

    private void SwordOnUnLoaded(object? sender, RoutedEventArgs e)
    {
        this.Classes.RemoveAll(this.Classes.ToList());
    }

    private void SwordOnRightAttacked(object? sender, RoutedEventArgs e)
    {
        this.Classes.RemoveAll(this.Classes.ToList());
        this.Classes.Add("RightAttack");
    }

    private void SwordOnLeftAttacked(object? sender, RoutedEventArgs e)
    {
        this.Classes.RemoveAll(this.Classes.ToList());
        this.Classes.Add("LeftAttack");
    }

    #region 依赖属性

    public static readonly StyledProperty<Bitmap> ImageSourceProperty = AvaloniaProperty.Register<
        Sword,
        Bitmap
    >(nameof(ImageSource));

    public Bitmap ImageSource
    {
        get { return GetValue(ImageSourceProperty); }
        set { SetValue(ImageSourceProperty, value); }
    }

    #endregion

    #region  事件

    public static readonly RoutedEvent<RoutedEventArgs> RightAttackedEvent = RoutedEvent.Register<
        Sword,
        RoutedEventArgs
    >(nameof(RightAttacked), RoutingStrategies.Direct);

    public event EventHandler<RoutedEventArgs> RightAttacked
    {
        add => AddHandler(RightAttackedEvent, value);
        remove => RemoveHandler(RightAttackedEvent, value);
    }

    public virtual void OnRightAttacked()
    {
        RoutedEventArgs args = new RoutedEventArgs(RightAttackedEvent);
        RaiseEvent(args);
    }

    public static readonly RoutedEvent<RoutedEventArgs> LeftAttackedEvent = RoutedEvent.Register<
        Sword,
        RoutedEventArgs
    >(nameof(LeftAttacked), RoutingStrategies.Direct);

    public event EventHandler<RoutedEventArgs> LeftAttacked
    {
        add => AddHandler(LeftAttackedEvent, value);
        remove => RemoveHandler(LeftAttackedEvent, value);
    }

    public virtual void OnLeftAttacked()
    {
        RoutedEventArgs args = new RoutedEventArgs(LeftAttackedEvent);
        RaiseEvent(args);
    }

    #endregion
}
