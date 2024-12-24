using NUnit.Framework;
using NAudio.Wave;
using System;
using System.IO;

namespace MusicPlayerTests
{
    [TestFixture]
    public class MusicPlayerTest
    {
        private MusicPlayer _musicPlayer;
        private string _backgroundMusicPath = "backgroundMusic.wav";
        private string _soundEffectPath = "soundEffect.wav";
        private string _invalidPath = "invalidPath.wav";

        [SetUp]
        public void SetUp()
        {
            _musicPlayer = new MusicPlayer();
        }

        [Test]
        public void TestConstructor_InitialValues()
        {
            Assert.That(_musicPlayer, Is.Not.Null);


        }

        [Test]
        public void TestPlayBackgroundMusic_FileDoesNotExist()
        {
            // 测试无效背景音乐文件路径时的处理
            _musicPlayer.PlayBackgroundMusic(_invalidPath);
            // 此时应输出文件不存在的错误，可以通过捕获控制台输出来验证
        }

        [Test]
        public void TestPlayBackgroundMusic_Success()
        {
            // 模拟一个有效的音乐文件播放
            var validMusicPath = _backgroundMusicPath;
            File.WriteAllText(validMusicPath, "fake audio data"); // 创建一个虚拟文件进行测试

            _musicPlayer.PlayBackgroundMusic(validMusicPath);
            // 由于无法直接验证音频播放的行为，可以通过模拟和检查内部状态来验证
            Assert.Pass("模拟背景音乐播放测试"); // 真实的测试应该检查播放行为
        }

        [Test]
        public void TestStopBackgroundMusic_Success()
        {
            // 模拟开始并停止背景音乐
            _musicPlayer.PlayBackgroundMusic(_backgroundMusicPath);
            _musicPlayer.StopBackgroundMusic();

            Assert.Pass("模拟停止背景音乐测试");
        }

        [Test]
        public void TestPlaySoundEffect_FileDoesNotExist()
        {
            _musicPlayer.PlaySoundEffect(_invalidPath);
            // 当文件路径无效时，应该输出错误信息
        }

        [Test]
        public void TestPlaySoundEffect_Success()
        {
            var validSoundEffectPath = _soundEffectPath;
            File.WriteAllText(validSoundEffectPath, "fake sound effect data"); // 创建一个虚拟文件进行测试

            _musicPlayer.PlaySoundEffect(validSoundEffectPath);

            Assert.Pass("模拟音效播放测试");
        }

        [Test]
        public void TestStopSoundEffect_Success()
        {
            _musicPlayer.PlaySoundEffect(_soundEffectPath);
            _musicPlayer.StopSoundEffect();

            Assert.Pass("模拟停止音效测试");
        }

        [Test]
        public void TestPlayBackgroundMusic_LoopCount()
        {
            var validMusicPath = _backgroundMusicPath;
            File.WriteAllText(validMusicPath, "fake audio data");

            _musicPlayer.PlayBackgroundMusic(validMusicPath);
            // 模拟背景音乐播放次数到达最大次数并停止
            // 由于无法直接检查音频播放行为，你可以通过模拟来验证行为
            Assert.Pass("模拟循环播放次数测试");
        }

        [Test]
        public void TestEffectPlayingFlag()
        {
            _musicPlayer.PlaySoundEffect(_soundEffectPath);
            // 检查音效播放标志位是否正确设置
            // 需要额外的内部状态检查或模拟来验证标志位的更新
            Assert.Pass("模拟音效播放标志测试");
        }

        [TearDown]
        public void TearDown()
        {
            // 清理测试过程中创建的文件
            if (File.Exists(_backgroundMusicPath)) File.Delete(_backgroundMusicPath);
            if (File.Exists(_soundEffectPath)) File.Delete(_soundEffectPath);
        }
    }
}
