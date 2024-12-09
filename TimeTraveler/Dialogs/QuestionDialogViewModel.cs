using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Irihi.Avalonia.Shared.Contracts;
using Ursa.Controls;

namespace TimeTraveler.Dialogs;

public partial class QuestionDialogViewModel : ObservableObject, IDialogContext
{
    public string AskedTitleText { get; set; } = "提示";

    public string AskedContentText { get; set; } = "";

    public QuestionDialogViewModel()
    {
        PrimaryButtonCommand = new RelayCommand(Primary);
        SecondaryButtonCommand = new RelayCommand(Secondary);
    }

    public void Close()
    {
        RequestClose?.Invoke(this, null);
    }

    public event EventHandler<object?>? RequestClose;

    public object PrimaryButtonCommandParameter { get; set; }

    public object SecondaryButtonCommandParameter { get; set; }

    public string PrimaryButtonContent { get; set; } = "确定";

    public string SecondaryButtonContent { get; set; } = "取消";
    public ICommand PrimaryButtonCommand { get; set; }
    public ICommand SecondaryButtonCommand { get; set; }

    public bool IsPrimaryButtonVisible { get; set; } = true;

    public bool IsSecondaryButtonVisible { get; set; } = true;

    private void Primary()
    {
        RequestClose?.Invoke(this, true);
    }

    private void Secondary()
    {
        RequestClose?.Invoke(this, false);
    }
}
