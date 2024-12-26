using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using CommunityToolkit.Mvvm.Input;
using TimeTraveler.Libary.Models;

namespace TimeTraveler.UserControls;

public class ThunderControl : TemplatedControl
{
    #region Properties

    public ICommand ClickCommand
    {
        get => GetValue(ClickCommandProperty);
        set => SetValue(ClickCommandProperty, value);
    }

    public static readonly StyledProperty<object> ClickCommandParameterProperty =
        AvaloniaProperty.Register<ThunderControl, object>(nameof(ClickCommandParameter));

    public object ClickCommandParameter
    {
        get => GetValue(ClickCommandParameterProperty);
        set => SetValue(ClickCommandParameterProperty, value);
    }

    public static readonly StyledProperty<ICommand> ClickCommandProperty =
        AvaloniaProperty.Register<ThunderControl, ICommand>(nameof(ClickCommand));

    public bool IsCompleted
    {
        get => GetValue(IsCompletedProperty);
        set => SetValue(IsCompletedProperty, value);
    }

    public static readonly StyledProperty<bool> IsCompletedProperty = AvaloniaProperty.Register<
        ThunderControl,
        bool
    >(nameof(IsCompleted));

    #endregion

    public ObservableCollection<BatteryModel> Batteries { get; private set; } =
        new ObservableCollection<BatteryModel>();

    public ICommand ClickBatteryCommand => new RelayCommand<BatteryModel>(OnBatteryClicked);

    public ThunderControl()
    {
        InitializeThunderControl();
    }

    private void InitializeThunderControl()
    {
        var batteries = Enumerable
            .Range(1, 5)
            .Select(i => new BatteryModel() { Id = i, Name = $"Battery {i}" })
            .ToList();
        var random = new Random();
        batteries[random.Next(0, batteries.Count)].IsHasElement = true;
        batteries.ForEach(Batteries.Add);
    }

    private void OnBatteryClicked(BatteryModel? obj)
    {
        obj.IsClicked = true;
        if (obj.IsHasElement)
        {
            IsCompleted = true;
        }
    }
}
