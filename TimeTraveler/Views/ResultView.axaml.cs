using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TimeTraveler.Dialogs;
using TimeTraveler.Libary.Helpers;
using TimeTraveler.Libary.Models;
using TimeTraveler.Libary.ViewModels;
using Notification = Ursa.Controls.Notification;
using WindowNotificationManager = Ursa.Controls.WindowNotificationManager;

namespace TimeTraveler.Views;

public partial class ResultView : UserControl
{
    private readonly ResultViewModel _viewModel;

    public ResultView()
    {
        _viewModel = ServiceLocator.Current.ResultViewModel;
        InitializeComponent();
        WeakReferenceMessenger.Default.Register<object, string>(this, "ShowMessage", ShowMessage);

        //给>>按钮订阅的属性加成的功能，当>>按钮被点击时候，也就是发布消息的时候，系统就会正在做属性加成的功能
        WeakReferenceMessenger.Default.Register<object, string>(
            this,
            "ShowResult",
            (sender, message) => ShowResult(_viewModel._resultModels)
        );
    }

    private void ShowMessage(object recipient, object message)
    {
        var viewModel = new QuestionDialogViewModel()
        {
            AskedContentText =
                "你已经完成3关试炼，可以进行下一关吗？或者继续试炼，一直到你探索找到答案为止？",
            AskedTitleText = "试炼询问",
            PrimaryButtonContent = "下一关",
            SecondaryButtonContent = "继续试炼",
            PrimaryButtonCommand = new RelayCommand<Tuple<QuestionDialogViewModel, object>>(
                GoToNextLevel
            ),
            SecondaryButtonCommand = new RelayCommand<Tuple<QuestionDialogViewModel, object>>(
                ContinueToExplore
            ),
        };
        viewModel.PrimaryButtonCommandParameter = Tuple.Create(viewModel, message);
        viewModel.SecondaryButtonCommandParameter = Tuple.Create(viewModel, message);
        DialogHelper.ShowDialog<QuestionDialog, QuestionDialogViewModel>(viewModel, false);
    }

    private void GoToNextLevel(object? obj)
    {
        if (obj is Tuple<QuestionDialogViewModel, object> tuple)
        {
            tuple.Item1?.Close();
            ShowResult(tuple.Item2);
        }
    }

    private void ContinueToExplore(object? obj)
    {
        if (obj is Tuple<QuestionDialogViewModel, object> tuple)
        {
            WeakReferenceMessenger.Default.Send<object, string>(0, "OnForwardNavigation");
            tuple.Item1?.Close();
        }
    }

    private void ShowResult(object? result)
    {
        if (result == null)
            return;
        var viewModel = new ResultSelectorDialogViewModel()
        {
            Results = result as ObservableCollection<ResultModel>,
            AskedTitleText = "属性加成",
            PrimaryButtonContent = "确认结果",
            IsSecondaryButtonVisible = false,
            PrimaryButtonCommand = new RelayCommand<Tuple<ResultSelectorDialogViewModel, object>>(
                ConfirmResult
            ),
        };
        viewModel.PrimaryButtonCommandParameter = Tuple.Create(viewModel, result);
        DialogHelper.ShowDialog<ResultSelectorDialog, ResultSelectorDialogViewModel>(
            viewModel,
            false
        );
    }

    private void ConfirmResult(object? obj)
    {
        if (obj is Tuple<ResultSelectorDialogViewModel, object> tuple)
        {
            if (!tuple.Item1.IsExistChecked())
            {
                ShowTip("请选择一个属性加成结果。");
                return;
            }

            var resultModels = tuple.Item2 as ObservableCollection<ResultModel>;
            var selectedResult = resultModels.FirstOrDefault(x => x.IsSelected);
            WeakReferenceMessenger.Default.Send<object, string>(true, "OnResultConfirmed");
            WeakReferenceMessenger.Default.Send<object, string>(new object(), "OnBackHome");
            WeakReferenceMessenger.Default.Send<object, string>(
                $"恭喜你，{selectedResult.Name}的属性加成成功！",
                "ShowNotification"
            );
            tuple.Item1?.Close();
        }
    }

    #region 消息通知提示窗

    public WindowNotificationManager? NotificationManager { get; set; }

    public void ShowTip(string message)
    {
        Enum.TryParse<NotificationType>("Warning", out var notificationType);
        NotificationManager?.Show(
            new Notification("警告", message),
            showIcon: true,
            showClose: true,
            type: notificationType
        );
        return;
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        var topLevel = TopLevel.GetTopLevel(this);
        NotificationManager = new WindowNotificationManager(topLevel)
        {
            MaxItems = 3,
            Position = NotificationPosition.TopLeft,
        };
    }

    protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromLogicalTree(e);
        NotificationManager?.Uninstall();
    }

    #endregion

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
