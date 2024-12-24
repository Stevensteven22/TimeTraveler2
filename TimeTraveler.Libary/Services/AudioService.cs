using Avalonia.Platform;
using Avalonia;
using NAudio.Wave;
using System.Reflection;

namespace TimeTraveler.Libary.Services;

public class AudioService : IAudioService
{
    private WaveOutEvent _waveOutEvent;
    private Mp3FileReader _mp3FileReader;
    private WaveOutEvent _backgroundMusicPlayer;
    private Mp3FileReader _backgroundMusicReader;
    
    
    public void PlayFlapSound()
    {
        // 确保之前的资源被释放
        _mp3FileReader?.Dispose();
        _waveOutEvent?.Dispose();


        var stream=  AssetLoader.Open(new Uri("avares://TimeTraveler/Assets/jumpSound.mp3"));
        // 加载并播放 MP3 音效
        _mp3FileReader = new Mp3FileReader(stream);
        _waveOutEvent = new WaveOutEvent();
        _waveOutEvent.Init(_mp3FileReader);
        _waveOutEvent.Play();
    }
    
    public void PlayBackgroundMusic()
    {
        // 如果背景音乐已经在播放，则不再重复播放
        if (_backgroundMusicPlayer != null && _backgroundMusicPlayer.PlaybackState == PlaybackState.Playing)
            return;

        // 确保已释放旧的资源
        _backgroundMusicReader?.Dispose();
        _backgroundMusicPlayer?.Dispose();

        var stream = AssetLoader.Open(new Uri("avares://TimeTraveler/Assets/GameThreeBackgroundMusuic.mp3"));
        // 创建新的播放器实例
        _backgroundMusicReader = new Mp3FileReader(stream);
        _backgroundMusicPlayer = new WaveOutEvent();
        _backgroundMusicPlayer.Init(_backgroundMusicReader);

        // 播放背景音乐
        _backgroundMusicPlayer.Play();

        // 处理播放停止事件
        _backgroundMusicPlayer.PlaybackStopped += (sender, e) =>
        {
            // 当音乐停止时释放资源
            _backgroundMusicReader?.Dispose();
            _backgroundMusicPlayer?.Dispose();
            _backgroundMusicPlayer = null;
            _backgroundMusicReader = null;
        };
    }

    public void StopBackgroundMusic()
    {
        // 确保播放器已初始化且正在播放
        if (_backgroundMusicPlayer != null && _backgroundMusicPlayer.PlaybackState == PlaybackState.Playing)
        {
            _backgroundMusicPlayer.Stop();
            _backgroundMusicReader?.Dispose();
            _backgroundMusicPlayer?.Dispose();
            _backgroundMusicPlayer = null;
            _backgroundMusicReader = null;
        }
    }
}