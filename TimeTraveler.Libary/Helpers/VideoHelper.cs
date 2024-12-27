using System.Collections.Concurrent;
using System.Diagnostics;
using Avalonia.Media.Imaging;
using OpenCvSharp;

namespace TimeTraveler.Libary.Helpers;

public class VideoHelper
{
    static Dictionary<int, CancellationTokenSource> taskCTS =
        new Dictionary<int, CancellationTokenSource>();
    static Dictionary<int, ConcurrentQueue<Mat>> matQueue =
        new Dictionary<int, ConcurrentQueue<Mat>>();
    private static int fps = 25;
    static List<RTSPURLInfo> play_rtsp_urls = new List<RTSPURLInfo>();

    private static bool isFirstlyPlay = true;

    /// <summary>
    /// 停止
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public static void StopToPlay(int index)
    {
        taskCTS[index]?.Cancel();
    }

    public static void Dispose(int index)
    {
        StopToPlay(index);
        if (matQueue.ContainsKey(index))
            matQueue.Remove(index);

        if (taskCTS.ContainsKey(index))
            taskCTS.Remove(index);

        play_rtsp_urls.RemoveAll(x => x.index == index);
    }

    public static void Initialize(
        string rtsp_url,
        int index,
        Action onPlayCompleted = null,
        EventHandler<StateChangedEventArgs> onPlayCompletedStateChanged = null
    )
    {
        //初始化
        taskCTS.TryAdd(index, new CancellationTokenSource());
        matQueue.TryAdd(index, new ConcurrentQueue<Mat>());

        if (!play_rtsp_urls.Any(x => x.index == index))
            play_rtsp_urls.Add(
                new RTSPURLInfo(index, rtsp_url, onPlayCompleted, onPlayCompletedStateChanged)
            );
    }

    public static void Update(int index, string rtsp_url, Action onPlayCompleted = null)
    {
        taskCTS[index] = new CancellationTokenSource();
        matQueue[index] = new ConcurrentQueue<Mat>();
        var play_rtsp_url = play_rtsp_urls.FirstOrDefault(x => x.index == index);
        if (play_rtsp_url == null)
            return;
        play_rtsp_url.url = rtsp_url;
        play_rtsp_url.OnPlayCompleted = onPlayCompleted;
    }

    /// <summary>
    /// 播放
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public static void Play(int index, bool isForever = false)
    {
        var play_rtsp_url = play_rtsp_urls.FirstOrDefault(x => x.index == index);
        if (play_rtsp_url == null)
            return;
        Task.Run(
            () =>
                DecodeRTSP(
                    play_rtsp_url.index,
                    matQueue[index],
                    play_rtsp_url.url,
                    taskCTS[index].Token,
                    isForever
                )
        );

        //显示线程
        /*if (isFirstlyPlay)
        {
            Task.Run(async () =>
            {
                int delay = (int)(1000 / fps);
                while (true)
                {
                    VideoPlayed?.Invoke(null, new VideoPlayedEventArgs() { Image = image, Index = index });
                    Cv2.WaitKey(delay);
                }
            });
            isFirstlyPlay = false;
        }*/
    }

    public static EventHandler<VideoPlayedEventArgs> VideoPlayed;

    public static Bitmap ImgShow(int index)
    {
        matQueue[index].TryDequeue(out Mat image);
        return ImageHelper.ToAvaloniaBitmap(image.ToMemoryStream());
    }

    static async void DecodeRTSP(
        int index,
        ConcurrentQueue<Mat> matQueue,
        string url,
        CancellationToken cancellationToken = default,
        bool isForever = false
    )
    {
        VideoCapture vcapture = new VideoCapture(url);
        if (!vcapture.IsOpened())
        {
            Console.WriteLine($"打开视频文件失败:{index}->{url}");
            return;
        }

        fps = (int)vcapture.Fps;
        // 计算等待时间（毫秒）
        int taskDelay = (int)Math.Round(1000 / vcapture.Fps);

        PlayCompletedStateChanged(index, false);
        while (true)
        {
            if (cancellationToken.IsCancellationRequested)
                break;

            Mat image = new Mat();
            if (!vcapture.Read(image))
            {
                Console.WriteLine($"{index}->{url}读取失败");
                if (isForever)
                {
                    if (image.Empty())
                    {
                        // 重置视频捕获到开始位置
                        vcapture.Set(VideoCaptureProperties.PosFrames, 0);
                        continue;
                    }
                }
                else
                {
                    PlayCompletedStateChanged(index, true);
                    PlayCompleted(index);
                    break;
                }
            }

            //matQueue.Enqueue(frame);
            VideoPlayed?.Invoke(null, new VideoPlayedEventArgs() { Image = image, Index = index });

            Cv2.WaitKey(taskDelay);
        }

        vcapture.Release();
    }

    private static void PlayCompletedStateChanged(int index, bool state)
    {
        var play_rtsp_url = play_rtsp_urls.FirstOrDefault(x => x.index == index);
        if (play_rtsp_url == null)
            return;
        play_rtsp_url.OnPlayCompletedStateChanged?.Invoke(
            null,
            new StateChangedEventArgs() { State = state, Index = index }
        );
    }

    private static void PlayCompleted(int index)
    {
        var play_rtsp_url = play_rtsp_urls.FirstOrDefault(x => x.index == index);
        if (play_rtsp_url == null)
            return;
        play_rtsp_url.OnPlayCompleted?.Invoke();
    }

    public static RTSPURLInfo GetInfo(int index)
    {
        return play_rtsp_urls.FirstOrDefault(x => x.index == index);
    }

    public class RTSPURLInfo
    {
        public int index;
        public string url;
        public Action OnPlayCompleted { get; set; }

        public EventHandler<StateChangedEventArgs> OnPlayCompletedStateChanged { get; set; }

        public RTSPURLInfo(
            int index,
            string url,
            Action onPlayCompleted = null,
            EventHandler<StateChangedEventArgs> onPlayCompletedStateChanged = null
        )
        {
            this.index = index;
            this.url = url;
            this.OnPlayCompleted = onPlayCompleted;
            this.OnPlayCompletedStateChanged = onPlayCompletedStateChanged;
        }
    }

    public class VideoPlayedEventArgs : EventArgs
    {
        public Mat Image { get; set; }
        public int Index { get; set; }
    }

    public class StateChangedEventArgs : EventArgs
    {
        public bool State { get; set; }
        public int Index { get; set; }
    }
}
