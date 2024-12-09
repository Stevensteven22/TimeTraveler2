using System;
using Avalonia;
using Avalonia.Controls.Primitives;

namespace TimeTraveler.UserControls;

public class ElectricCurrent : TemplatedControl
{
    public bool IsShowed
    {
        get => GetValue(IsShowedProperty);
        set => SetValue(IsShowedProperty, value);
    }

    public static readonly StyledProperty<bool> IsShowedProperty = AvaloniaProperty.Register<
        ElectricCurrent,
        bool
    >(nameof(IsShowed), false);

    public bool IsCompleted
    {
        get => GetValue(IsCompletedProperty);
        set => SetValue(IsCompletedProperty, value);
    }

    public static readonly StyledProperty<bool> IsCompletedProperty = AvaloniaProperty.Register<
        ElectricCurrent,
        bool
    >(nameof(IsCompleted), false);

    public double CenterY
    {
        get => GetValue(CenterYProperty);
        set => SetValue(CenterYProperty, value);
    }

    public static readonly StyledProperty<double> CenterYProperty = AvaloniaProperty.Register<
        ElectricCurrent,
        double
    >(nameof(CenterY), 0d);
    public double CenterX
    {
        get => GetValue(CenterXProperty);
        set => SetValue(CenterXProperty, value);
    }

    public static readonly StyledProperty<double> CenterXProperty = AvaloniaProperty.Register<
        ElectricCurrent,
        double
    >(nameof(CenterX), 0d);

    public double SkewAngleY
    {
        get => GetValue(SkewAngleYProperty);
        set => SetValue(SkewAngleYProperty, value);
    }

    public static readonly StyledProperty<double> SkewAngleYProperty = AvaloniaProperty.Register<
        ElectricCurrent,
        double
    >(nameof(SkewAngleY), 0d);
    public double SkewAngleX
    {
        get => GetValue(SkewAngleXProperty);
        set => SetValue(SkewAngleXProperty, value);
    }

    public static readonly StyledProperty<double> SkewAngleXProperty = AvaloniaProperty.Register<
        ElectricCurrent,
        double
    >(nameof(SkewAngleX), 0d);
    public double OffsetY
    {
        get => GetValue(OffsetYProperty);
        set => SetValue(OffsetYProperty, value);
    }

    public static readonly StyledProperty<double> OffsetYProperty = AvaloniaProperty.Register<
        ElectricCurrent,
        double
    >(nameof(OffsetY), 0d);
    public double OffsetX
    {
        get => GetValue(OffsetXProperty);
        set => SetValue(OffsetXProperty, value);
    }

    public static readonly StyledProperty<double> OffsetXProperty = AvaloniaProperty.Register<
        ElectricCurrent,
        double
    >(nameof(OffsetX), 0d);
    public double ScaleX
    {
        get => GetValue(ScaleXProperty);
        set => SetValue(ScaleXProperty, value);
    }

    public static readonly StyledProperty<double> ScaleXProperty = AvaloniaProperty.Register<
        ElectricCurrent,
        double
    >(nameof(ScaleX), 1d);

    public double ScaleY
    {
        get => GetValue(ScaleYProperty);
        set => SetValue(ScaleYProperty, value);
    }

    public static readonly StyledProperty<double> ScaleYProperty = AvaloniaProperty.Register<
        ElectricCurrent,
        double
    >(nameof(ScaleY), 1d);

    public double RotationAngle
    {
        get => GetValue(RotationAngleProperty);
        set => SetValue(RotationAngleProperty, value);
    }

    public static readonly StyledProperty<double> RotationAngleProperty = AvaloniaProperty.Register<
        ElectricCurrent,
        double
    >(nameof(RotationAngle), 0d);

    public double ArrowBodyHeight
    {
        get => GetValue(ArrowBodyHeightProperty);
        set => SetValue(ArrowBodyHeightProperty, value);
    }

    public static readonly StyledProperty<double> ArrowBodyHeightProperty =
        AvaloniaProperty.Register<ElectricCurrent, double>(nameof(ArrowBodyHeight), 40d);
    public double ArrowBodyWidth
    {
        get => GetValue(ArrowBodyWidthProperty);
        set => SetValue(ArrowBodyWidthProperty, value);
    }

    public static readonly StyledProperty<double> ArrowBodyWidthProperty =
        AvaloniaProperty.Register<ElectricCurrent, double>(nameof(ArrowBodyWidth), 700d);

    public double ArrowHeaderSize
    {
        get => GetValue(ArrowHeaderSizeProperty);
        set => SetValue(ArrowHeaderSizeProperty, value);
    }

    public static readonly StyledProperty<double> ArrowHeaderSizeProperty =
        AvaloniaProperty.Register<ElectricCurrent, double>(nameof(ArrowHeaderSize), 140d);

    public ElectricCurrent()
    {
        this.GetObservable(IsVisibleProperty)
            .Subscribe(
                (newValue) =>
                {
                    this.IsShowed = newValue;
                }
            );
    }
}
