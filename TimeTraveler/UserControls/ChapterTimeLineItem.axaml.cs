using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Ursa.Controls;

namespace TimeTraveler.UserControls;

public enum ChapterTimelineItemPosition
{
    Top,
    Bottom,
    Separate,
}

[PseudoClasses(PC_AllTop, PC_AllBottom, PC_Separate, PC_IsOK)]
public class ChapterTimelineItem : TimelineItem
{
    private const string PC_IsOK = ":isOK";
    private const string PC_AllTop = ":all-top";
    private const string PC_AllBottom = ":all-bottom";
    public static new readonly StyledProperty<ChapterTimelineItemPosition> PositionProperty =
        AvaloniaProperty.Register<ChapterTimelineItem, ChapterTimelineItemPosition>(
            nameof(Position),
            defaultValue: ChapterTimelineItemPosition.Top
        );

    public ChapterTimelineItemPosition Position
    {
        get => GetValue(PositionProperty);
        set => SetValue(PositionProperty, value);
    }

    public bool IsOK
    {
        get => GetValue(IsOKProperty);
        set => SetValue(IsOKProperty, value);
    }

    public static readonly StyledProperty<bool> IsOKProperty = AvaloniaProperty.Register<
        ChapterTimelineItem,
        bool
    >(nameof(IsOK), defaultValue: false);

    public ChapterTimelineItem()
    {
        PositionProperty.Changed.AddClassHandler<ChapterTimelineItem, ChapterTimelineItemPosition>(
            (item, args) =>
            {
                item.OnModeChanged(args);
            }
        );

        IsOKProperty.Changed.AddClassHandler<ChapterTimelineItem, bool>(
            (item, args) =>
            {
                item.OnIsOKChanged(args);
            }
        );
    }

    private void OnModeChanged(AvaloniaPropertyChangedEventArgs<ChapterTimelineItemPosition> args)
    {
        SetMode(args.NewValue.Value);
    }

    private void OnIsOKChanged(AvaloniaPropertyChangedEventArgs<bool> args)
    {
        PseudoClasses.Set(PC_IsOK, args.NewValue.Value);
    }

    private void SetMode(ChapterTimelineItemPosition mode)
    {
        PseudoClasses.Set(PC_AllTop, mode == ChapterTimelineItemPosition.Top);
        PseudoClasses.Set(PC_AllBottom, mode == ChapterTimelineItemPosition.Bottom);
        PseudoClasses.Set(PC_Separate, mode == ChapterTimelineItemPosition.Separate);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        SetMode(Position);
    }

    internal void SetIfUnset<T>(AvaloniaProperty<T> property, T value)
    {
        if (!IsSet(property))
        {
            SetCurrentValue(property, value);
        }
    }
}
