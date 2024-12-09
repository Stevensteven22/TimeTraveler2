using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Metadata;
using Ursa.Controls;

namespace TimeTraveler.UserControls;

public enum ChapterTimeLineDisplayMode
{
    Top,
    Center,
    Bottom,
    Alternate,
}

public class ChapterTimeLine : Timeline
{
    public ChapterTimeLine() { }

    public static readonly StyledProperty<IBinding?> IsOKMemberBindingProperty =
        AvaloniaProperty.Register<ChapterTimeLine, IBinding?>(nameof(IsOKMemberBinding));

    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IBinding? IsOKMemberBinding
    {
        get => GetValue(IsOKMemberBindingProperty);
        set => SetValue(IsOKMemberBindingProperty, value);
    }

    public static new readonly StyledProperty<ChapterTimeLineDisplayMode> ModeProperty =
        AvaloniaProperty.Register<ChapterTimeLine, ChapterTimeLineDisplayMode>(nameof(Mode));

    public ChapterTimeLineDisplayMode Mode
    {
        get => GetValue(ModeProperty);
        set => SetValue(ModeProperty, value);
    }

    static ChapterTimeLine()
    {
        ModeProperty.Changed.AddClassHandler<ChapterTimeLine, ChapterTimeLineDisplayMode>(
            (t, e) =>
            {
                t.OnDisplayModeChanged(e);
            }
        );
    }

    private void OnDisplayModeChanged(
        AvaloniaPropertyChangedEventArgs<ChapterTimeLineDisplayMode> e
    )
    {
        SetItemMode((ChapterTimeLineDisplayMode)e.NewValue.Value);
    }

    private void SetItemMode(ChapterTimeLineDisplayMode mode)
    {
        if (ItemsPanelRoot is TimelinePanel panel)
        {
            var items = panel.Children.OfType<ChapterTimeLine>();
            if (Mode == ChapterTimeLineDisplayMode.Top)
            {
                foreach (var item in items)
                {
                    SetIfUnset(
                        item,
                        ChapterTimelineItem.PositionProperty,
                        ChapterTimelineItemPosition.Top
                    );
                }
            }
            else if (Mode == ChapterTimeLineDisplayMode.Bottom)
            {
                foreach (var item in items)
                {
                    SetIfUnset(
                        item,
                        ChapterTimelineItem.PositionProperty,
                        ChapterTimelineItemPosition.Bottom
                    );
                }
            }
            else if (Mode == ChapterTimeLineDisplayMode.Center)
            {
                foreach (var item in items)
                {
                    SetIfUnset(
                        item,
                        ChapterTimelineItem.PositionProperty,
                        ChapterTimelineItemPosition.Separate
                    );
                }
            }
            else if (Mode == ChapterTimeLineDisplayMode.Alternate)
            {
                var top = false;
                foreach (var item in items)
                {
                    SetIfUnset(
                        item,
                        ChapterTimelineItem.PositionProperty,
                        top ? ChapterTimelineItemPosition.Top : ChapterTimelineItemPosition.Bottom
                    );
                    top = !top;
                }
            }
        }
    }

    private void SetIfUnset<T>(AvaloniaObject target, StyledProperty<T> property, T value)
    {
        if (!target.IsSet(property))
            target.SetCurrentValue(property, value);
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        recycleKey = null;
        return item is not ChapterTimelineItem;
    }

    protected override Control CreateContainerForItemOverride(
        object? item,
        int index,
        object? recycleKey
    )
    {
        if (item is ChapterTimelineItem t)
            return t;
        return new ChapterTimelineItem();
    }

    protected override void PrepareContainerForItemOverride(
        Control container,
        object? item,
        int index
    )
    {
        base.PrepareContainerForItemOverride(container, item, index);
        if (container is ChapterTimelineItem t)
        {
            if (IsOKMemberBinding is not null)
            {
                t.Bind(ChapterTimelineItem.IsOKProperty, IsOKMemberBinding);
            }
            if (IconMemberBinding is not null)
            {
                t.Bind(ChapterTimelineItem.IconProperty, IconMemberBinding);
            }
            if (HeaderMemberBinding != null)
            {
                t.Bind(HeaderedContentControl.HeaderProperty, HeaderMemberBinding);
            }
            if (ContentMemberBinding != null)
            {
                t.Bind(ContentControl.ContentProperty, ContentMemberBinding);
            }
            if (TimeMemberBinding != null)
            {
                t.Bind(ChapterTimelineItem.TimeProperty, TimeMemberBinding);
            }

            t.SetIfUnset(ChapterTimelineItem.TimeFormatProperty, TimeFormat);
            t.SetIfUnset(ChapterTimelineItem.IconTemplateProperty, IconTemplate);
            t.SetIfUnset(HeaderedContentControl.HeaderTemplateProperty, ItemTemplate);
            t.SetIfUnset(ContentControl.ContentTemplateProperty, DescriptionTemplate);
        }
    }
}
