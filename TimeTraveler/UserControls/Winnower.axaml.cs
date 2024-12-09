using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using TimeTraveler.Libary.Definitions;
using TimeTraveler.Libary.Models;

namespace TimeTraveler.UserControls;

public class Winnower : TemplatedControl
{
    public bool IsRotateCompleted
    {
        get => GetValue(IsRotateCompletedProperty);
        set => SetValue(IsRotateCompletedProperty, value);
    }

    public static readonly StyledProperty<bool> IsRotateCompletedProperty =
        AvaloniaProperty.Register<Winnower, bool>(nameof(IsRotateCompleted));

    public ObservableCollection<WindModel> WindDirections { get; private set; } =
        new ObservableCollection<WindModel>();

    public WindModel WinnowerDirection
    {
        get => GetValue(WinnowerDirectionProperty);
        set => SetValue(WinnowerDirectionProperty, value);
    }

    public static readonly StyledProperty<WindModel> WinnowerDirectionProperty =
        AvaloniaProperty.Register<Winnower, WindModel>(nameof(WinnowerDirection));

    public double WindSpeed
    {
        get => GetValue(WindSpeedProperty);
        set => SetValue(WindSpeedProperty, value);
    }

    public static readonly StyledProperty<double> WindSpeedProperty = AvaloniaProperty.Register<
        Winnower,
        double
    >(nameof(WindSpeed));

    public Image PART_Winnower =>
        this.GetTemplateChildren().FirstOrDefault(e => e.Name == "PART_Winnower") as Image;

    public Winnower()
    {
        WinnowerDirectionProperty.Changed.AddClassHandler<Winnower>(
            OnWinnowerDirectionPropertyChanged
        );
        InitializeWinnowerControl();
    }

    public void InitializeWinnowerControl()
    {
        var winds = new List<WindModel>()
        {
            new WindModel() { Direction = WindDirection.None },
            new WindModel() { Direction = WindDirection.WindClockwiseSlowly },
            new WindModel() { Direction = WindDirection.WindAnticlockwiseSlowly },
            new WindModel() { Direction = WindDirection.WindClockwiseFastly },
            new WindModel() { Direction = WindDirection.WindAnticlockwiseFastly },
            new WindModel() { Direction = WindDirection.WindCompletelyStopped },
        };
        var random = new Random();
        winds[random.Next(0, winds.Count)].IsHasElement = true;
        winds.ForEach(WindDirections.Add);
    }

    private static void OnWinnowerDirectionPropertyChanged(
        Winnower sender,
        AvaloniaPropertyChangedEventArgs e
    )
    {
        // 'e' 参数描述了发生的更改。
        if (e.NewValue is WindModel newValue)
        {
            if (e.OldValue is WindModel oldValue)
                sender.PART_Winnower.Classes.Remove(oldValue.Direction.ToString());

            sender.PART_Winnower.Classes.Add(newValue.Direction.ToString());

            switch (newValue.Direction)
            {
                case WindDirection.None:
                    sender.WindSpeed = 0;
                    break;
                case WindDirection.WindClockwiseSlowly:
                    sender.WindSpeed = 100;
                    break;
                case WindDirection.WindAnticlockwiseSlowly:
                    sender.WindSpeed = 100;
                    break;
                case WindDirection.WindClockwiseFastly:
                    sender.WindSpeed = 300;
                    break;
                case WindDirection.WindAnticlockwiseFastly:
                    sender.WindSpeed = 300;
                    break;
                case WindDirection.WindCompletelyStopped:
                    sender.WindSpeed = 200;
                    break;
            }

            if (newValue.IsHasElement)
                sender.IsRotateCompleted = true;
        }
    }
}
