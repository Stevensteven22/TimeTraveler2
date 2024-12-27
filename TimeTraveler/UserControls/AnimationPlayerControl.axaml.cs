using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using OpenCvSharp;
using TimeTraveler.Libary.Helpers;
using Ursa.Controls;

namespace TimeTraveler.UserControls;

public class AnimationPlayerControl : TemplatedControl
{
    protected string _resourceUri = string.Empty;

    public static readonly StyledProperty<bool> IsPlayCompletedProperty = AvaloniaProperty.Register<
        AnimationPlayerControl,
        bool
    >(nameof(IsPlayCompleted));

    public bool IsPlayCompleted
    {
        get { return GetValue(IsPlayCompletedProperty); }
        set { SetValue(IsPlayCompletedProperty, value); }
    }

    public static readonly StyledProperty<bool> IsForeverProperty = AvaloniaProperty.Register<
        AnimationPlayerControl,
        bool
    >(nameof(IsForever));

    public bool IsForever
    {
        get { return GetValue(IsForeverProperty); }
        set { SetValue(IsForeverProperty, value); }
    }

    public static readonly StyledProperty<int> IndexProperty = AvaloniaProperty.Register<
        AnimationPlayerControl,
        int
    >(nameof(Index));

    public int Index
    {
        get { return GetValue(IndexProperty); }
        set { SetValue(IndexProperty, value); }
    }

    public static readonly StyledProperty<Bitmap> ImageSourceProperty = AvaloniaProperty.Register<
        AnimationPlayerControl,
        Bitmap
    >(nameof(ImageSource));

    public Bitmap ImageSource
    {
        get { return GetValue(ImageSourceProperty); }
        set { SetValue(ImageSourceProperty, value); }
    }

    public static readonly StyledProperty<string> ResourceUriProperty = AvaloniaProperty.Register<
        AnimationPlayerControl,
        string
    >(nameof(ResourceUri), "");

    public string ResourceUri
    {
        get { return GetValue(ResourceUriProperty); }
        set { SetValue(ResourceUriProperty, value); }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
    }

    public AnimationPlayerControl()
    {
        ResourceUriProperty.Changed.AddClassHandler<AnimationPlayerControl>(
            OnResourceUriPropertyChanged
        );

        VideoHelper.VideoPlayed += (sender, args) =>
        {
            Dispatcher.UIThread.Invoke(() =>
            {
                this.OnVideoPlayed(args.Image, args.Index);
            });
        };

        this.VideoPlayed += (sender, args) =>
        {
            // 显示当前帧
            if (args.Index == this.Index)
                this.ImageSource = ImageHelper.ToAvaloniaBitmap(args.Image.ToMemoryStream());
        };

        this.Unloaded += (sender, args) =>
        {
            //VideoHelper.StopToPlay(this.Index);
            VideoHelper.Dispose(this.Index);
        };

        this.Loaded += (sender, args) =>
        {
            if (!string.IsNullOrEmpty(this._resourceUri))
            {
                VideoHelper.Initialize(
                    this._resourceUri,
                    this.Index,
                    onPlayCompletedStateChanged: (sender, args) =>
                    {
                        Dispatcher.UIThread.Invoke(() =>
                        {
                            if (args.Index == this.Index)
                                IsPlayCompleted = args.State;
                        });
                    }
                );
                // 开始播放视频
                VideoHelper.Play(this.Index, this.IsForever);
            }
        };
    }

    private static async void OnResourceUriPropertyChanged(
        object sender,
        AvaloniaPropertyChangedEventArgs e
    )
    {
        var control = sender as AnimationPlayerControl;
        if (
            control != null
            && e.NewValue is string resourceUri
            && !string.IsNullOrEmpty(resourceUri)
        )
        {
            // 获取实际的文件路径
            var assetUri = new Uri(resourceUri);

            string tempFilePath = string.Empty;
            using (var assetStream = AssetLoader.Open(assetUri))
            {
                // 将资源流复制到临时文件
                tempFilePath = Path.GetTempFileName();
                using (
                    var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write)
                )
                {
                    await assetStream.CopyToAsync(fileStream);
                }
            }

            if (string.IsNullOrEmpty(tempFilePath))
                return; // 文件路径为空，无法播放视频

            control._resourceUri = tempFilePath;
        }
    }

    #region  事件

    public static readonly RoutedEvent<VideoPlayedRoutedEventArgs> VideoPlayedEvent =
        RoutedEvent.Register<AnimationPlayerControl, VideoPlayedRoutedEventArgs>(
            nameof(VideoPlayed),
            RoutingStrategies.Direct
        );

    public event EventHandler<VideoPlayedRoutedEventArgs> VideoPlayed
    {
        add => AddHandler(VideoPlayedEvent, value);
        remove => RemoveHandler(VideoPlayedEvent, value);
    }

    protected virtual void OnVideoPlayed(Mat image, int index)
    {
        if (image != null)
        {
            VideoPlayedRoutedEventArgs args = new VideoPlayedRoutedEventArgs();
            args.RoutedEvent = VideoPlayedEvent;
            args.Image = image;
            args.Index = index;
            RaiseEvent(args);
        }
    }

    #endregion


    public class VideoPlayedRoutedEventArgs : RoutedEventArgs
    {
        public Mat Image { get; set; }

        public int Index { get; set; }
    }
}
