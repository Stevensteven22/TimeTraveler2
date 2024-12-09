using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.VisualTree;
using TimeTraveler.Libary.Definitions;

namespace TimeTraveler.Transitions;

public class FadeToTransition : UserControl, IPageTransition
{
    public FadeToTransition() { }

    public FadeToTransition(TimeSpan duration)
    {
        Duration = duration;
    }

    /// <summary>
    /// 获取或设置动画的持续时间。
    /// </summary>
    public TimeSpan Duration { get; set; }

    public static readonly StyledProperty<FadeModeType> FadeModeProperty =
        AvaloniaProperty.Register<FadeToTransition, FadeModeType>(
            nameof(FadeMode),
            FadeModeType.OneWay
        );

    public FadeModeType FadeMode
    {
        get => GetValue(FadeModeProperty);
        set => SetValue(FadeModeProperty, value);
    }

    public async Task Start(
        Visual from,
        Visual to,
        bool forward,
        CancellationToken cancellationToken
    )
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        var parent = GetVisualParent(from, to);
        if (to != null && !cancellationToken.IsCancellationRequested)
        {
            to.IsVisible = false;
        }

        if (from != null)
        {
            var animation = new Animation
            {
                FillMode = FillMode.Backward,
                Children =
                {
                    new KeyFrame
                    {
                        Setters =
                        {
                            new Setter { Property = OpacityProperty, Value = 1d },
                        },
                        Cue = new Cue(0d),
                    },
                    new KeyFrame
                    {
                        Setters =
                        {
                            new Setter
                            {
                                Property = OpacityProperty,
                                Value = FadeModeType.OneWay == FadeMode ? 1d : 0.8d,
                            },
                        },
                        Cue = new Cue(1d),
                    },
                },
                Duration = Duration,
            };
            await animation.RunAsync(from, cancellationToken);
        }

        if (from != null && !cancellationToken.IsCancellationRequested)
        {
            from.IsVisible = FadeModeType.OneWay == FadeMode ? true : false;
        }

        if (to != null)
        {
            to.IsVisible = true;
            var animation = new Animation
            {
                FillMode = FillMode.Forward,
                Children =
                {
                    new KeyFrame
                    {
                        Setters =
                        {
                            new Setter { Property = OpacityProperty, Value = 0.8d },
                        },
                        Cue = new Cue(0d),
                    },
                    new KeyFrame
                    {
                        Setters =
                        {
                            new Setter { Property = OpacityProperty, Value = 1d },
                        },
                        Cue = new Cue(1d),
                    },
                },
                Duration = Duration,
            };
            await animation.RunAsync(to, cancellationToken);
        }
    }

    /// <summary>
    /// 获取两个控件的共同视觉父级。
    /// </summary>
    /// <param name="from">源控件。</param>
    /// <param name="to">目标控件。</param>
    /// <returns>共同的父级。</returns>
    /// <exception cref="ArgumentException">
    /// 两个控件没有共同的父级。
    /// </exception>
    /// <remarks>
    /// 任何一个参数可能为null，但不能都为null。
    /// </remarks>
    private static Visual GetVisualParent(Visual? from, Visual? to)
    {
        var p1 = (from ?? to)!.GetVisualParent();
        var p2 = (to ?? from)!.GetVisualParent();

        if (p1 != null && p2 != null && p1 != p2)
        {
            throw new ArgumentException("Controls for PageSlide must have same parent.");
        }

        return p1 ?? throw new InvalidOperationException("Cannot determine visual parent.");
    }
}
