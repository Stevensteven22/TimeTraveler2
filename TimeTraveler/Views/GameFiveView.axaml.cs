using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using NAudio.Wave;

namespace TimeTraveler.Views;

public partial class GameFiveView : UserControl
{
    private WaveOutEvent _waveOutEvent;
    private Mp3FileReader _mp3FileReader;

    public GameFiveView()
    {
        InitializeComponent();

        this.Loaded += GameFiveView_Loaded;

        this.Unloaded += (sender, args) =>
        {
            StopToPlayBackgroundSound();
        };
    }

    private void GameFiveView_Loaded(object? sender, RoutedEventArgs e)
    {
        PlayBackgroundSound();

        GameBoss.PropertyChanged += (sender, e) =>
        {
            if (e.Property.Name == "IsPlayCompleted")
            {
                if (_isBossAttacking)
                {
                    this.AttackButton.IsEnabled = e.GetNewValue<bool>();
                    _isBossAttacking = false;
                }
            }
        };
    }

    private void StopToPlayBackgroundSound()
    {
        _waveOutEvent?.Stop();
    }

    private void PlayBackgroundSound()
    {
        _mp3FileReader?.Dispose();
        _waveOutEvent?.Dispose();

        var stream = AssetLoader.Open(new Uri("avares://TimeTraveler/Assets/关卡4背景音乐.mp3"));
        // 加载并播放 MP3 音效
        _mp3FileReader = new Mp3FileReader(stream);
        _waveOutEvent = new WaveOutEvent();
        _waveOutEvent.Init(_mp3FileReader);
        _waveOutEvent.Play();
    }

    private bool _isBossAttacking;

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        _isBossAttacking = true;
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
}
