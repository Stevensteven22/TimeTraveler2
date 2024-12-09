using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Irihi.Avalonia.Shared.Contracts;
using TimeTraveler.Libary.Models;

namespace TimeTraveler.Dialogs;

public partial class ResultSelectorDialogViewModel : ObservableObject, IDialogContext
{
    public ObservableCollection<ResultModel> Results { get; set; } = new();

    public string AskedTitleText { get; set; } = "提示";

    public string AskedContentText { get; set; } = "";

    public ResultSelectorDialogViewModel()
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

    [RelayCommand]
    public void IsCheckedChanged(CheckBox checkBox)
    {
        if (checkBox.IsChecked == true)
        {
            foreach (var result in Results)
            {
                if ((int)checkBox.Tag != result.Id)
                    result.IsSelected = false;
            }
        }
    }

    public bool IsExistChecked()
    {
        return Results.Any(x => x.IsSelected);
    }
}
