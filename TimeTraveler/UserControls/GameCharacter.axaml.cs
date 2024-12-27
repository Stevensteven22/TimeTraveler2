using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Platform;
using Avalonia.Threading;
using TimeTraveler.Libary.Helpers;

namespace TimeTraveler.UserControls;

public class GameCharacter : AnimationPlayerControl
{
    private string _knockedBackResourceUri = string.Empty;

    public static readonly StyledProperty<string> KnockedBackResourceUriProperty =
        AvaloniaProperty.Register<GameCharacter, string>(nameof(KnockedBackResourceUri));

    public string KnockedBackResourceUri
    {
        get { return GetValue(KnockedBackResourceUriProperty); }
        set { SetValue(KnockedBackResourceUriProperty, value); }
    }

    public static readonly StyledProperty<bool> IsKnockedBackProperty = AvaloniaProperty.Register<
        GameCharacter,
        bool
    >(nameof(IsKnockedBack));

    public bool IsKnockedBack
    {
        get { return GetValue(IsKnockedBackProperty); }
        set { SetValue(IsKnockedBackProperty, value); }
    }

    protected override Type StyleKeyOverride => typeof(AnimationPlayerControl);

    private bool _isFirst=true;
    public GameCharacter()
    {
        /*IsKnockedBackProperty.Changed.AddClassHandler<GameCharacter>(
            OnIsKnockedBackPropertyChanged
        );*/

        KnockedBackResourceUriProperty.Changed.AddClassHandler<GameCharacter>(
            OnKnockedBackResourceUriPropertyChanged
        );
        
        this.GetObservable(IsKnockedBackProperty)
            .Subscribe(async isKnockedBack =>
            {
                if (_isFirst)
                {
                    _isFirst = false;
                    return;
                }
                if (isKnockedBack)
                {
                    VideoHelper.StopToPlay(this.Index);
                    VideoHelper.Update(
                        this.Index,
                        this._knockedBackResourceUri,
                        onPlayCompleted: () =>
                        {
                            Dispatcher.UIThread.Invoke(() =>
                            {
                                this.IsKnockedBack = false;
                            });
                        }
                    );
                    VideoHelper.Play(this.Index, false);
                }
                else
                {
                    VideoHelper.StopToPlay(this.Index);
                    VideoHelper.Update(this.Index, this._resourceUri);
                    VideoHelper.Play(this.Index, this.IsForever);
                }
                
            });
    }

    private static async void OnKnockedBackResourceUriPropertyChanged(
        GameCharacter sender,
        AvaloniaPropertyChangedEventArgs e
    )
    {
        var control = sender as GameCharacter;
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

            control._knockedBackResourceUri = tempFilePath;
        }
    }

    private static void OnIsKnockedBackPropertyChanged(
        GameCharacter sender,
        AvaloniaPropertyChangedEventArgs e
    )
    {
        var control = sender as GameCharacter;
        if (control != null && e.NewValue is bool isKnockedBack)
        {
            if (isKnockedBack)
            {
                VideoHelper.StopToPlay(control.Index);
                VideoHelper.Update(
                    control.Index,
                    control._knockedBackResourceUri,
                    onPlayCompleted: () =>
                    {
                        Dispatcher.UIThread.Invoke(() =>
                        {
                            control.IsKnockedBack = false;
                        });
                    }
                );
                VideoHelper.Play(control.Index, false);
            }
            else
            {
                VideoHelper.StopToPlay(control.Index);
                VideoHelper.Update(control.Index, control._resourceUri);
                VideoHelper.Play(control.Index, control.IsForever);
            }
        }
    }
}
