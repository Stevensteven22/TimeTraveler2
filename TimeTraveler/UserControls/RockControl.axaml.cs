using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.Input;
using TimeTraveler.Libary.Models;

namespace TimeTraveler.UserControls;

public partial class RockControl : UserControl
{
    #region Properties
    
    public ICommand ClickCommand
    {
        get => GetValue(ClickCommandProperty);
        set => SetValue(ClickCommandProperty, value);
    }

    public static readonly StyledProperty<object> ClickCommandParameterProperty =
        AvaloniaProperty.Register<RockControl, object>(nameof(ClickCommandParameter));

    public object ClickCommandParameter
    {
        get => GetValue(ClickCommandParameterProperty);
        set => SetValue(ClickCommandParameterProperty, value);
    }

    public static readonly StyledProperty<ICommand> ClickCommandProperty =
        AvaloniaProperty.Register<RockControl, ICommand>(nameof(ClickCommand));


    public bool IsCompleted
    {
        get => GetValue(IsCompletedProperty);
        set => SetValue(IsCompletedProperty, value);
    }

    public static readonly StyledProperty<bool> IsCompletedProperty = AvaloniaProperty.Register<
        RockControl,
        bool
    >(nameof(IsCompleted));

    #endregion
    public RockControl()
    {
        InitializeComponent();
        InitializeRockControl();
    }

    public ObservableCollection<RockModel> RockItems { get; private set; } =
        new ObservableCollection<RockModel>();

    private void InitializeRockControl()
    {
        var rocks = Enumerable
            .Range(1, 9)
            .Select(i => new RockModel() { Id = i, Name = $"Rock {i}" })
            .ToList();
        var random = new Random();
        rocks[random.Next(0, rocks.Count)].IsHasElement = true;
        rocks.ForEach(RockItems.Add);
    }

    public ICommand RockClickCommand => new RelayCommand<RockModel>(OnRockClicked);
    
    private void OnRockClicked(RockModel rock)
    {
        if (rock.IsHasElement)
        {
            IsCompleted = true;
        }
    }
}
