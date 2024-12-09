using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.VisualTree;

namespace TimeTraveler.Transitions;

public class SwitchTransition : IPageTransition
{
    /// <summary>
    /// 初始化 <see cref="SwitchTransition"/> 类的新实例。
    /// </summary>
    public SwitchTransition() { }

    /// <summary>
    /// 初始化 <see cref="SwitchTransition"/> 类的新实例。
    /// </summary>
    /// <param name="duration">The duration of the animation.</param>
    public SwitchTransition(TimeSpan duration)
    {
        Duration = duration;
    }

    /// <summary>
    /// 获取或设置动画的持续时间。
    /// </summary>
    public TimeSpan Duration { get; set; }

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
