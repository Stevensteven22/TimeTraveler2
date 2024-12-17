using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging;
using TimeTraveler.UserControls;
using Notification = Ursa.Controls.Notification;
using WindowNotificationManager = Ursa.Controls.WindowNotificationManager;

namespace TimeTraveler.Views;

public partial class BackgroundTwoView : UserControl
{
    public BackgroundTwoView()
    {
        InitializeComponent();
      
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