using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace TimeTraveler.Views;

public partial class BackgroundFiveView : UserControl
{
    public BackgroundFiveView()
    {
        InitializeComponent();
    }


    private void Button_PointerEntered_1(object? sender, Avalonia.Input.PointerEventArgs e)
    {
        if (sender is Button button)
        {
            button.Background = Brushes.AliceBlue;
        }
    }

    private void Button_PointerExited_2(object? sender, Avalonia.Input.PointerEventArgs e)
    {
        if (sender is Button button)
        {
            button.Background = new SolidColorBrush(Color.Parse("#E9E5D9"));
        }
    }
}