using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace TimeTraveler.UserControls;

public class IceSlider : TemplatedControl
{
    public double Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public static readonly StyledProperty<double> ValueProperty = AvaloniaProperty.Register<
        IceSlider,
        double
    >(nameof(Value));

    public bool IsMeltCompleted
    {
        get => GetValue(IsMeltCompletedProperty);
        set => SetValue(IsMeltCompletedProperty, value);
    }

    public static readonly StyledProperty<bool> IsMeltCompletedProperty = AvaloniaProperty.Register<
        IceSlider,
        bool
    >(nameof(IsMeltCompleted));

    public ICommand ClickCommand
    {
        get => GetValue(ClickCommandProperty);
        set => SetValue(ClickCommandProperty, value);
    }

    public static readonly StyledProperty<object> ClickCommandParameterProperty =
        AvaloniaProperty.Register<IceSlider, object>(nameof(ClickCommandParameter));

    public object ClickCommandParameter
    {
        get => GetValue(ClickCommandParameterProperty);
        set => SetValue(ClickCommandParameterProperty, value);
    }

    public static readonly StyledProperty<ICommand> ClickCommandProperty =
        AvaloniaProperty.Register<IceSlider, ICommand>(nameof(ClickCommand));

    public IceSlider()
    {
        this.PropertyChanged += (sender, e) =>
        {
            if (e.Property == ValueProperty && e.NewValue is double newValue)
            {
                if (Math.Round(newValue) == 80d)
                    IsMeltCompleted = true;
            }
        };
    }
}
