using System.Reflection;
using Moq;
using NAudio.Wave;
using NUnit.Framework;

namespace MusicPlayerTests
{
    [TestFixture]
    public class MusicPlayerTest_3
    {
        private MusicPlayer _musicPlayer;

        [SetUp]
        public void SetUp()
        {
            _musicPlayer = new MusicPlayer();
        }

        [Test]
        public void PlaySoundEffect_FileDoesNotExist_ShouldNotThrow()
        {
            Assert.DoesNotThrow(() => _musicPlayer.PlaySoundEffect("nonexistent.wav"));
        }

        [Test]
        public void PlaySoundEffect_WhenEffectIsAlreadyPlaying_ShouldReturnImmediately()
        {
            // 使用 Mock 创建虚假的音效播放器，模拟音效正在播放
            var mockEffectPlayer = new Mock<IWavePlayer>();
            var mockEffectReader = new Mock<AudioFileReader>("Assets/Move.wav");

            var musicPlayer = new MusicPlayer(effectPlayer: mockEffectPlayer.Object,
                effectReader: mockEffectReader.Object);

            // 手动设置 _isEffectPlaying 为 true
            var isEffectPlayingField =
                typeof(MusicPlayer).GetField("_isEffectPlaying", BindingFlags.NonPublic | BindingFlags.Instance);
            isEffectPlayingField.SetValue(musicPlayer, true);

            // 调用 PlaySoundEffect
            musicPlayer.PlaySoundEffect("Assets/Move.wav");

            // 验证 Play 方法未被调用，因为正在播放时应立即返回
            mockEffectPlayer.Verify(player => player.Play(), Times.Never);
        }

        [Test]
        public void PlaySoundEffect_ShouldCatchExceptions()
        {
            // 使用 Mock 模拟异常
            var mockEffectPlayer = new Mock<IWavePlayer>();
            mockEffectPlayer.Setup(player => player.Init(It.IsAny<AudioFileReader>()))
                .Throws(new Exception("Test exception"));

            var musicPlayer = new MusicPlayer(effectPlayer: mockEffectPlayer.Object);

            Assert.DoesNotThrow(() => musicPlayer.PlaySoundEffect("Assets/Move.wav"));
        }

        [Test]
        public void PlaySoundEffect_NormalPlayback_ShouldTriggerPlaybackStopped()
        {
            // Mock 对象
            var mockEffectPlayer = new Mock<IWavePlayer>();
            var mockEffectReader = new Mock<AudioFileReader>("Assets/Move.wav");

            var musicPlayer = new MusicPlayer(effectPlayer: mockEffectPlayer.Object,
                effectReader: mockEffectReader.Object);

            // 设置 Mock 行为
            mockEffectPlayer.Setup(player => player.Play()).Callback(() =>
            {
                // 模拟 PlaybackStopped 事件触发
                mockEffectPlayer.Raise(player => player.PlaybackStopped += null, EventArgs.Empty);
            });

            // 调用方法
            musicPlayer.PlaySoundEffect("Assets/Move.wav");

            // 验证 Play 方法是否被调用


            // 验证资源释放逻辑
            mockEffectPlayer.Verify(player => player.Stop(), Times.Once);
            mockEffectPlayer.Verify(player => player.Dispose(), Times.Once);
        }

    }
}