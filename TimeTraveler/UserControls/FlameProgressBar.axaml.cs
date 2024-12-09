using System.Linq;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;

namespace TimeTraveler.UserControls;

public class FlameProgressBar : TemplatedControl
{
    public static readonly StyledProperty<object> ClickCommandParameterProperty =
        AvaloniaProperty.Register<FlameProgressBar, object>(nameof(ClickCommandParameter));

    public object ClickCommandParameter
    {
        get => GetValue(ClickCommandParameterProperty);
        set => SetValue(ClickCommandParameterProperty, value);
    }
    public ICommand ClickCommand
    {
        get => GetValue(ClickCommandProperty);
        set => SetValue(ClickCommandProperty, value);
    }

    public static readonly StyledProperty<ICommand> ClickCommandProperty =
        AvaloniaProperty.Register<FlameProgressBar, ICommand>(nameof(ClickCommand));

    private void SetPseudoclasses(string name, bool flag)
    {
        PseudoClasses.Set($":{name}", flag);
    }

    public ToggleButton PART_Button1 =>
        this.GetTemplateChildren().FirstOrDefault(e => e.Name == "PART_Button1") as ToggleButton;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
    }
}
