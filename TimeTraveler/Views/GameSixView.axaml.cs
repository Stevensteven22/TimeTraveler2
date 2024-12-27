using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using NAudio.Wave;
using TimeTraveler.Dialogs;
using TimeTraveler.Libary.Helpers;

namespace TimeTraveler.Views;

public partial class GameSixView : UserControl
{
    public GameSixView()
    {
        InitializeComponent();

        //this.Loaded += GameFiveView_Loaded;

        WeakReferenceMessenger.Default.Register<object, string>(
            this,
            "OnCharacterAttacked",
            (sender, message) =>
            {
                LeftSword.OnLeftAttacked();
                GameBoss.IsKnockedBack = true;
                //BossHealthBar.Value -= 250;
                BossDeductHPText.OnTextFlown();
            }
        );

        WeakReferenceMessenger.Default.Register<object, string>(
            this,
            "OnBossAttacking",
            (sender, message) =>
            {
                RightSword.OnRightAttacked();
            }
        );

        WeakReferenceMessenger.Default.Register<object, string>(
            this,
            "TextFlyout",
            (sender, message) =>
            {
                if (message.ToString() == "Character")
                    CharacterDeductHPText.OnTextFlown();
                else if (message.ToString() == "Boss")
                    BossDeductHPText.OnTextFlown();
            }
        );

        WeakReferenceMessenger.Default.Register<object, string>(
            this,
            "OnBossAttacked",
            (sender, message) =>
            {
                RightSword.OnRightAttacked();
                GameCharacter.IsKnockedBack = true;
                //CharacterHealthBar.Value -= 30;
                CharacterDeductHPText.OnTextFlown();
            }
        );

        WeakReferenceMessenger.Default.Register<object, string>(
            this,
            "OnBossDefeated",
            (sender, message) =>
            {
                ShowMessage(
                    sender,
                    "游戏胜利",
                    "恭喜成功击败了守护笔记本的Boss，解开了时间旅行者的密室。"
                );
            }
        );

        WeakReferenceMessenger.Default.Register<object, string>(
            this,
            "OnCharacterDefeated",
            (sender, message) =>
            {
                ShowMessage(
                    sender,
                    "游戏失败",
                    "很遗憾，您未能通过时间旅行者的考验，你冒险的路还未停止..."
                );
            }
        );
    }

    private void GameFiveView_Loaded(object? sender, RoutedEventArgs e)
    {
        /*GameBoss.PropertyChanged += (sender, e) =>
        {
            if (e.Property.Name == "IsPlayCompleted")
            {
                if (_isCharacterAttacking)
                {
                    this.AttackButton.IsEnabled = e.GetNewValue<bool>();
                    _isCharacterAttacking = false;
                }
            }
        };*/
    }

    private bool _isCharacterAttacking;

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        _isCharacterAttacking = true;
        LeftSword.OnLeftAttacked();
        GameBoss.IsKnockedBack = true;
        BossHealthBar.Value -= 10;
        this.AttackButton.IsEnabled = false;
    }

    private void Button_OnClick2(object? sender, RoutedEventArgs e)
    {
        RightSword.OnRightAttacked();
        GameCharacter.IsKnockedBack = true;
        CharacterHealthBar.Value -= 10;
    }

    private void Button_OnClick3(object? sender, RoutedEventArgs e)
    {
        GameCharacter.IsKnockedBack = false;
        GameBoss.IsKnockedBack = false;
    }

    private void ShowMessage(object recipient, object promptMessage, object? callbackParameter)
    {
        var viewModel = new QuestionDialogViewModel()
        {
            AskedContentText = promptMessage.ToString(),
            AskedTitleText = "提示",
            IsPrimaryButtonVisible = false,
            SecondaryButtonContent = "继续",
            SecondaryButtonCommand = new RelayCommand<Tuple<QuestionDialogViewModel, object>>(
                ContinueTo
            ),
        };
        viewModel.SecondaryButtonCommandParameter = Tuple.Create(viewModel, callbackParameter);
        DialogHelper.ShowDialog<QuestionDialog, QuestionDialogViewModel>(viewModel, false);
    }

    private void ContinueTo(object? obj)
    {
        if (obj is Tuple<QuestionDialogViewModel, object> tuple)
        {
            WeakReferenceMessenger.Default.Send<object, string>(1, "OnForwardNavigation");
            WeakReferenceMessenger.Default.Send<object, string>(
                new { ReturnText = tuple.Item2, IsGoToStartView = true },
                "OnReturnViewArrived"
            );
            tuple.Item1?.Close();
        }
    }
}
