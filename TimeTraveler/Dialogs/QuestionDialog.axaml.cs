using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TimeTraveler.UserControls;

namespace TimeTraveler.Dialogs;

public partial class QuestionDialog : UserControl
{
    public object SecondaryButtonContent
    {
        get => GetValue(SecondaryButtonContentProperty);
        set => SetValue(SecondaryButtonContentProperty, value);
    }

    public static readonly StyledProperty<object> SecondaryButtonContentProperty =
        AvaloniaProperty.Register<QuestionDialog, object>(nameof(SecondaryButtonContent), "取消");
    public ICommand SecondaryButtonCommand
    {
        get => GetValue(SecondaryButtonCommandProperty);
        set => SetValue(SecondaryButtonCommandProperty, value);
    }

    public static readonly StyledProperty<ICommand> SecondaryButtonCommandProperty =
        AvaloniaProperty.Register<QuestionDialog, ICommand>(nameof(SecondaryButtonCommand));

    public object SecondaryButtonCommandParameter
    {
        get => GetValue(SecondaryButtonCommandParameterProperty);
        set => SetValue(SecondaryButtonCommandParameterProperty, value);
    }

    public static readonly StyledProperty<object> SecondaryButtonCommandParameterProperty =
        AvaloniaProperty.Register<QuestionDialog, object>(nameof(SecondaryButtonCommandParameter));

    public object PrimaryButtonContent
    {
        get => GetValue(PrimaryButtonContentProperty);
        set => SetValue(PrimaryButtonContentProperty, value);
    }

    public static readonly StyledProperty<object> PrimaryButtonContentProperty =
        AvaloniaProperty.Register<QuestionDialog, object>(nameof(PrimaryButtonContent), "确定");
    public ICommand PrimaryButtonCommand
    {
        get => GetValue(PrimaryButtonCommandProperty);
        set => SetValue(PrimaryButtonCommandProperty, value);
    }

    public static readonly StyledProperty<ICommand> PrimaryButtonCommandProperty =
        AvaloniaProperty.Register<QuestionDialog, ICommand>(nameof(PrimaryButtonCommand));

    public object PrimaryButtonCommandParameter
    {
        get => GetValue(PrimaryButtonCommandParameterProperty);
        set => SetValue(PrimaryButtonCommandParameterProperty, value);
    }

    public static readonly StyledProperty<object> PrimaryButtonCommandParameterProperty =
        AvaloniaProperty.Register<QuestionDialog, object>(nameof(PrimaryButtonCommandParameter));

    public string AskedTitleText
    {
        get => GetValue(AskedTitleTextProperty);
        set => SetValue(AskedTitleTextProperty, value);
    }

    public static readonly StyledProperty<string> AskedTitleTextProperty =
        AvaloniaProperty.Register<QuestionDialog, string>(nameof(AskedTitleText), "提示");

    public string AskedContentText
    {
        get => GetValue(AskedContentTextProperty);
        set => SetValue(AskedContentTextProperty, value);
    }

    public static readonly StyledProperty<string> AskedContentTextProperty =
        AvaloniaProperty.Register<QuestionDialog, string>(nameof(AskedContentText));

    public QuestionDialog()
    {
        InitializeComponent();
    }
}
