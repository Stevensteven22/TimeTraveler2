using System;
using System.Linq;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using TimeTraveler.Libary.Definitions;

namespace TimeTraveler.UserControls;

public class ShowElementalControl : TemplatedControl
{
    public ICommand ClickCommand
    {
        get => GetValue(ClickCommandProperty);
        set => SetValue(ClickCommandProperty, value);
    }

    public static readonly StyledProperty<object> ClickCommandParameterProperty =
        AvaloniaProperty.Register<ShowElementalControl, object>(nameof(ClickCommandParameter));

    public object ClickCommandParameter
    {
        get => GetValue(ClickCommandParameterProperty);
        set => SetValue(ClickCommandParameterProperty, value);
    }

    public static readonly StyledProperty<ICommand> ClickCommandProperty =
        AvaloniaProperty.Register<ShowElementalControl, ICommand>(nameof(ClickCommand));
    public bool IsShowEnabled
    {
        get => GetValue(IsShowEnabledProperty);
        set => SetValue(IsShowEnabledProperty, value);
    }

    public static readonly StyledProperty<bool> IsShowEnabledProperty = AvaloniaProperty.Register<
        ShowElementalControl,
        bool
    >(nameof(IsShowEnabled));

    public string ElementalContentText
    {
        get => GetValue(ElementalContentTextProperty);
        set => SetValue(ElementalContentTextProperty, value);
    }

    public static readonly StyledProperty<string> ElementalContentTextProperty =
        AvaloniaProperty.Register<ShowElementalControl, string>(nameof(ElementalContentText));

    public ElementType ElementalImage
    {
        get => GetValue(ElementalImageProperty);
        set => SetValue(ElementalImageProperty, value);
    }

    public static readonly StyledProperty<ElementType> ElementalImageProperty =
        AvaloniaProperty.Register<ShowElementalControl, ElementType>(nameof(ElementalImage));

    public Button PART_Element =>
        this.GetTemplateChildren().FirstOrDefault(e => e.Name == "PART_Element") as Button;

    public ShowElementalControl()
    {
        this.GetObservable(ElementalImageProperty)
            .Subscribe(newValue =>
            {
                switch (newValue)
                {
                    case ElementType.FireElemental:
                        ElementalContentText = "火";
                        break;
                    case ElementType.IceElemental:
                        ElementalContentText = "冰";
                        break;
                    case ElementType.WindElemental:
                        ElementalContentText = "风";
                        break;
                    case ElementType.RockElemental:
                        ElementalContentText = "岩";
                        break;
                    case ElementType.ThunderElemental:
                        ElementalContentText = "雷";
                        break;
                }
            });
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        PART_Element.Click += PART_Element_Click;
    }

    private void PART_Element_Click(object? sender, RoutedEventArgs e)
    {
        ClickCommand?.Execute(ClickCommandParameter);
    }
}
