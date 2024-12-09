using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Avalonia.Threading;
using TimeTraveler.Libary.Definitions;

namespace TimeTraveler.UserControls;

[PseudoClasses(":isAnimationEnabled", ":isClicked")]
public class Rock : TemplatedControl
{
    #region Additional Properties and Methods

    public bool IsHasGoldOre
    {
        get => GetValue(IsHasGoldOreProperty);
        set => SetValue(IsHasGoldOreProperty, value);
    }

    public static readonly StyledProperty<bool> IsHasGoldOreProperty = AvaloniaProperty.Register<
        Rock,
        bool
    >(nameof(IsHasGoldOre));

    public string RockName
    {
        get => GetValue(RockNameProperty);
        set => SetValue(RockNameProperty, value);
    }

    public static readonly StyledProperty<string> RockNameProperty = AvaloniaProperty.Register<
        Rock,
        string
    >(nameof(RockName));

    #endregion

    #region Basic Properties and Methods

    public object SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public static readonly StyledProperty<object> SelectedItemProperty = AvaloniaProperty.Register<
        Rock,
        object
    >(nameof(SelectedItem));

    public ICommand ClickCommand
    {
        get => GetValue(ClickCommandProperty);
        set => SetValue(ClickCommandProperty, value);
    }

    public static readonly StyledProperty<object> ClickCommandParameterProperty =
        AvaloniaProperty.Register<Rock, object>(nameof(ClickCommandParameter));

    public object ClickCommandParameter
    {
        get => GetValue(ClickCommandParameterProperty);
        set => SetValue(ClickCommandParameterProperty, value);
    }

    public static readonly StyledProperty<ICommand> ClickCommandProperty =
        AvaloniaProperty.Register<Rock, ICommand>(nameof(ClickCommand));

    public static readonly RoutedEvent<RoutedEventArgs> ClickedEvent = RoutedEvent.Register<
        Rock,
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
        AnimationCompleted += () =>
        {
            OnClicked();
            ClickCommand?.Execute(ClickCommandParameter);
        };
        PART_ContentControl.Click += async (sender, args) =>
        {
            await AnimateToDo();
        };
    }

    private Action AnimationCompleted;

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

    private async Task<bool> AnimateToDo()
    {
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
                        SetPseudoclasses("isClicked", false);
                        AnimationCompleted?.Invoke();
                    });
                }

                return t.Result;
            });
    }

    private void SetPseudoclasses(string name, bool flag)
    {
        PseudoClasses.Set($":{name}", flag);
    }

    #endregion
}
