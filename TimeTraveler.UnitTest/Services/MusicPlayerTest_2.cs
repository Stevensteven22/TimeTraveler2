using Moq;
using NUnit.Framework;
using NAudio.Wave;
using System;
using System.Reflection;


namespace MusicPlayerTests
{
    [TestFixture]
    public class MusicPlayerTest_2
    {
        private Mock<IWavePlayer> _mockBackgroundMusicPlayer;
        private Mock<AudioFileReader> _mockBackgroundMusicReader;
        private MusicPlayer _musicPlayer;

        [SetUp]
        public void SetUp()
        {
            _mockBackgroundMusicPlayer = new Mock<IWavePlayer>();
            _musicPlayer = new MusicPlayer(_mockBackgroundMusicPlayer.Object, new AudioFileReader("Assets/Move.wav"));
        }

        [Test]
        public void TestBackgroundMusicPlayback_StopsAfterMaxPlayCount()
        {
            // 启动背景音乐播放
            _musicPlayer.PlayBackgroundMusic("Assets/Move.wav");

            // 使用反射访问 private 字段 _playCount
            var playCountField = typeof(MusicPlayer).GetField("_playCount", BindingFlags.NonPublic | BindingFlags.Instance);
            var playCount = (int)playCountField.GetValue(_musicPlayer);

            // 验证播放次数
            Assert.AreEqual(1, playCount); // 第一次播放

            // 模拟第二次播放
            _mockBackgroundMusicPlayer.Raise(p => p.PlaybackStopped += null, new StoppedEventArgs());

            // 再次使用反射获取 playCount
            playCount = (int)playCountField.GetValue(_musicPlayer);

            // 验证第二次播放
            Assert.AreEqual(1, playCount); // 第二次播放

            // 模拟停止
            _mockBackgroundMusicPlayer.Raise(p => p.PlaybackStopped += null, new StoppedEventArgs());

            // 最后一次获取 playCount
            playCount = (int)playCountField.GetValue(_musicPlayer);

            // 验证播放停止
            Assert.AreEqual(1, playCount); // 达到最大播放次数，播放应该停止
        }
    }
}