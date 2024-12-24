using NAudio.Wave;
using System;
using System.IO;

public class MusicPlayer
{
    private IWavePlayer _backgroundMusicPlayer;
    private AudioFileReader _backgroundMusicReader;
    private int _playCount; // 背景音乐播放次数
    private readonly int _maxPlayCount = 2; // 背景音乐最大播放次数
    private readonly object _backgroundMusicLock = new object();

    private IWavePlayer _effectPlayer;
    private AudioFileReader _effectReader;
    private readonly object _effectLock = new object();
    private bool _isEffectPlaying = false;
    
    public MusicPlayer(IWavePlayer backgroundMusicPlayer = null, AudioFileReader backgroundMusicReader = null, 
        IWavePlayer effectPlayer = null, AudioFileReader effectReader = null)
    {
        _backgroundMusicPlayer = backgroundMusicPlayer ?? new WaveOutEvent();
        _backgroundMusicReader = backgroundMusicReader;
        _effectPlayer = effectPlayer ?? new WaveOutEvent();
        _effectReader = effectReader;
    }

    /// <summary>
    /// 播放背景音乐，支持循环播放。
    /// </summary>
    /// <param name="musicFilePath">背景音乐文件路径</param>
    public void PlayBackgroundMusic(string musicFilePath)
    {
        if (!File.Exists(musicFilePath))
        {
            Console.WriteLine($"文件不存在：{musicFilePath}");
            return;
        }

        try
        {
            StopBackgroundMusic();

            lock (_backgroundMusicLock)
            {
                _backgroundMusicPlayer = new WaveOutEvent();
                _backgroundMusicReader = new AudioFileReader(musicFilePath);
                _backgroundMusicPlayer.Init(_backgroundMusicReader);

                _backgroundMusicPlayer.PlaybackStopped += (sender, args) =>
                {
                    if (_playCount < _maxPlayCount)
                    {
                        _backgroundMusicReader.Position = 0; // 重置音频位置
                        _backgroundMusicPlayer.Play(); // 重新播放
                        _playCount++;
                    }
                    else
                    {
                        StopBackgroundMusic();
                    }
                };

                _backgroundMusicPlayer.Play();
                _playCount = 1;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"播放背景音乐时发生错误：{ex.Message}");
        }
    }

    /// <summary>
    /// 停止背景音乐，并释放资源。
    /// </summary>
    public void StopBackgroundMusic()
    {
        lock (_backgroundMusicLock)
        {
            if (_backgroundMusicPlayer != null)
            {
                _backgroundMusicPlayer.PlaybackStopped -= null;
                _backgroundMusicPlayer.Stop();
                _backgroundMusicPlayer.Dispose();
                _backgroundMusicPlayer = null;
            }

            _backgroundMusicReader?.Dispose();
            _backgroundMusicReader = null;
            _playCount = 0;
        }
    }

    /// <summary>
    /// 播放音效。
    /// </summary>
    /// <param name="soundFilePath">音效文件路径</param>
    public void PlaySoundEffect(string soundFilePath)
    {
        if (!File.Exists(soundFilePath))
        {
            Console.WriteLine($"文件不存在：{soundFilePath}");
            return;
        }

        try
        {
            if (_isEffectPlaying) return;

            StopSoundEffect();
            _isEffectPlaying = true;

            lock (_effectLock)
            {
                _effectPlayer = new WaveOutEvent();
                _effectReader = new AudioFileReader(soundFilePath);
                _effectPlayer.Init(_effectReader);

                _effectPlayer.PlaybackStopped += (sender, args) =>
                {
                    StopSoundEffect();
                    _isEffectPlaying = false;
                };

                _effectPlayer.Play();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"播放音效时发生错误：{ex.Message}");
        }
    }

    /// <summary>
    /// 停止音效播放，并释放资源。
    /// </summary>
    public void StopSoundEffect()
    {
        lock (_effectLock)
        {
            if (_effectPlayer != null)
            {
                _effectPlayer.PlaybackStopped -= null;
                _effectPlayer.Stop();
                _effectPlayer.Dispose();
                _effectPlayer = null;
            }

            _effectReader?.Dispose();
            _effectReader = null;
        }
    }
}
