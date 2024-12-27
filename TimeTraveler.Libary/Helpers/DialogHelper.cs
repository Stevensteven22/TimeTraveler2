using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Irihi.Avalonia.Shared.Contracts;
using TimeTraveler.Libary.ViewModels;
using Ursa.Controls;

namespace TimeTraveler.Libary.Helpers;

public static class DialogHelper
{
    public static void ShowDialog<TView, TViewModel>(
        TViewModel viewModel = null,
        bool isCloseButtonVisible = true
    )
        where TView : UserControl, new()
        where TViewModel : ObservableObject, IDialogContext
    {
        var options = new DialogOptions()
        {
            Title = string.Empty,
            Mode = DialogMode.None,
            Button = DialogButton.None,
            ShowInTaskBar = true,
            IsCloseButtonVisible = isCloseButtonVisible,
            StartupLocation = WindowStartupLocation.CenterScreen,
            CanDragMove = true,
            CanResize = false,
        };
        if (viewModel == null)
            viewModel = Activator.CreateInstance<TViewModel>();

        Dialog.ShowCustom<TView, TViewModel>(viewModel, options: options);
    }
}
