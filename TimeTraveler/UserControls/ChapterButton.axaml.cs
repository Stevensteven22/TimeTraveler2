using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;

namespace TimeTraveler.UserControls;

[PseudoClasses(":isSelected", ":isOK")]
public class ChapterButton : ToggleButton
{
    public Thickness ButtonBorderPadding
    {
        get => GetValue(ButtonBorderPaddingProperty);
        set => SetValue(ButtonBorderPaddingProperty, value);
    }

    public static readonly StyledProperty<Thickness> ButtonBorderPaddingProperty = AvaloniaProperty.Register<
        ChapterButton,
        Thickness
    >(nameof(ButtonBorderPadding), new Thickness(3));
    
    public Thickness ButtonBorderThickness
    {
        get => GetValue(ButtonBorderThicknessProperty);
        set => SetValue(ButtonBorderThicknessProperty, value);
    }

    public static readonly StyledProperty<Thickness> ButtonBorderThicknessProperty = AvaloniaProperty.Register<
        ChapterButton,
        Thickness
    >(nameof(ButtonBorderThickness), new Thickness(2));
    
    public bool IsOK
    {
        get => GetValue(IsOKProperty);
        set => SetValue(IsOKProperty, value);
    }

    public static readonly StyledProperty<bool> IsOKProperty = AvaloniaProperty.Register<
        ChapterButton,
        bool
    >(nameof(IsOK), defaultValue: false);

    public ChapterButton()
    {
        IsOKProperty.Changed.AddClassHandler<ChapterButton>(IsOKPropertyChanged);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        if (this.IsOK)
            this.SetPseudoclasses("isOK", true);
        else
            this?.SetPseudoclasses("isOK", false);
    }

    private static void IsOKPropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
    {
        var control = sender as ChapterButton;
        if (control != null && control.IsOK)
            control.SetPseudoclasses("isOK", true);
        else
            control?.SetPseudoclasses("isOK", false);
    }

 
    private void SetPseudoclasses(string name, bool flag)
    {
        PseudoClasses.Set($":{name}", flag);
    }
}
