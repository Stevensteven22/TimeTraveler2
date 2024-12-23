using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using TimeTraveler.Libary.ViewModels;
using TimeTraveler.Services;

namespace TimeTraveler.Views
{
    public partial class GameFourView : UserControl
    {
        public GameFourView()
        {
            InitializeComponent();
            this.DataContext = new GameFourViewModel(new ResultVerifyService()); // 根据实际的验证服务传入
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        private void Button_PointerEntered_1(object? sender, Avalonia.Input.PointerEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                button.Background = Brushes.AliceBlue;
            }
        }

        private void Button_PointerExited_2(object? sender, Avalonia.Input.PointerEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                button.Background = new SolidColorBrush(Color.Parse("#E9E5D9"));
            }
        }
    }
}