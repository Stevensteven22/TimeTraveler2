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

namespace TimeTraveler.UserControls;

[PseudoClasses(":isAnimationCompleted", ":isClicked")]
public class Ice : TemplatedControl
{
    public bool IsMeltCompleted
    {
        get => GetValue(IsMeltCompletedProperty);
        set => SetValue(IsMeltCompletedProperty, value);
    }

    public static readonly StyledProperty<bool> IsMeltCompletedProperty = AvaloniaProperty.Register<
        Ice,
        bool
    >(nameof(IsMeltCompleted));
    public object SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public static readonly StyledProperty<object> SelectedItemProperty = AvaloniaProperty.Register<
        Ice,
        object
    >(nameof(SelectedItem));

    public double Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public static readonly StyledProperty<double> ValueProperty = AvaloniaProperty.Register<
        Ice,
        double
    >(nameof(Value));

    public ICommand ClickCommand
    {
        get => GetValue(ClickCommandProperty);
        set => SetValue(ClickCommandProperty, value);
    }

    public static readonly StyledProperty<object> ClickCommandParameterProperty =
        AvaloniaProperty.Register<Ice, object>(nameof(ClickCommandParameter));

    public object ClickCommandParameter
    {
        get => GetValue(ClickCommandParameterProperty);
        set => SetValue(ClickCommandParameterProperty, value);
    }

    public static readonly StyledProperty<ICommand> ClickCommandProperty =
        AvaloniaProperty.Register<Ice, ICommand>(nameof(ClickCommand));

    public static readonly RoutedEvent<RoutedEventArgs> ClickedEvent = RoutedEvent.Register<
        Ice,
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

    public Ice() { }

    private void OnGValuePropertyChanged(Ice arg1, AvaloniaPropertyChangedEventArgs arg2) { }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        //ValueProperty.Changed.AddClassHandler<Ice>(OnGValuePropertyChanged);

        this.GetObservable(ValueProperty)
            .Subscribe(async newValue =>
            {
                switch (Math.Round(newValue))
                {
                    case 0d:
                        PART_AnimationList.SelectedIndex = 0;
                        break;
                    case 10d:
                        PART_AnimationList.SelectedIndex = 1;
                        break;
                    case 20d:
                        PART_AnimationList.SelectedIndex = 2;
                        break;
                    case 30d:
                        PART_AnimationList.SelectedIndex = 3;
                        break;
                    case 40d:
                        PART_AnimationList.SelectedIndex = 4;
                        break;
                    case 50d:
                        PART_AnimationList.SelectedIndex = 5;
                        break;
                    case 60d:
                        PART_AnimationList.SelectedIndex = 6;
                        break;
                    case 70d:
                        PART_AnimationList.SelectedIndex = 7;
                        break;
                    case 80d:
                        await AnimateToMelt();
                        break;
                }
            });

        //PART_AnimationList.Loaded += PART_AnimationList_OnLoaded;
        PART_AnimationList.SelectedIndex = 0;
        MeltCompleted += () =>
        {
            SetPseudoclasses("isAnimationCompleted", true);
        };
        PART_ContentControl.Click += async (sender, args) =>
        {
            await OnPressedToUpdateUI(() =>
            {
                OnClicked();
                ClickCommand?.Execute(ClickCommandParameter);
            });
        };
    }

    private Task OnPressedToUpdateUI(Action completed = null)
    {
        return Task.Run(async () =>
            {
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    SetPseudoclasses("isClicked", true);
                });
            })
            .ContinueWith(async t =>
            {
                await Task.Delay(300);
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    SetPseudoclasses("isClicked", false);
                    completed?.Invoke();
                });
            });
    }

    private Action MeltCompleted;

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
            if (selectedIndex == PART_AnimationList.Items.Count)
                selectedIndex = 0;
        };
        _timer.Start();
    }

    private async Task<bool> AnimateToMelt()
    {
        return await Task.Run(async () =>
            {
                int selectedIndex = 8;
                for (
                    selectedIndex = 8;
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
                        MeltCompleted?.Invoke();
                        IsMeltCompleted = true;
                    });
                }

                return t.Result;
            });
    }

    private void SetPseudoclasses(string name, bool flag)
    {
        PseudoClasses.Set($":{name}", flag);
    }
}
