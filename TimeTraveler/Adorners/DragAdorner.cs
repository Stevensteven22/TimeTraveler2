using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Layout;
using Avalonia.Media;

namespace TimeTraveler.Adorners;

public class DragAdorner : Decorator
{
    public DragAdorner(Point pos, Control control)
    {
        IsHitTestVisible = false;

       

        this.Child = control;

        double left = pos.X - (150 / 2);
        double top = pos.Y - (75 / 2);
        this.RenderTransformOrigin = RelativePoint.Center;
        this.RenderTransform = new TranslateTransform(left, top);
    }
}
