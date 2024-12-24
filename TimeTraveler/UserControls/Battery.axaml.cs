using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using Avalonia.VisualTree;
using TimeTraveler.Adorners;

namespace TimeTraveler.UserControls;

[PseudoClasses(":isClicked")]
public class Battery : Button
{
    #region Properties

    public bool IsSelected
    {
        get => GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    public static readonly StyledProperty<bool> IsSelectedProperty = AvaloniaProperty.Register<
        Battery,
        bool
    >(nameof(IsSelected));

    public bool IsLeftSelected
    {
        get => GetValue(IsLeftSelectedProperty);
        set => SetValue(IsLeftSelectedProperty, value);
    }

    public static readonly StyledProperty<bool> IsLeftSelectedProperty = AvaloniaProperty.Register<
        Battery,
        bool
    >(nameof(IsLeftSelected));

    #endregion

    public Battery()
    {
        this.GetObservable(IsLeftSelectedProperty)
            .Subscribe(newValue =>
            {
                if (newValue)
                {
                    this.Classes.Add("left-selected");
                    //this.AddHandler(PointerMovedEvent, OnBatteryMoved, RoutingStrategies.Tunnel);
                    this.AddHandler(
                        PointerPressedEvent,
                        OnBatteryPressed,
                        RoutingStrategies.Tunnel
                    );
                    /*this.AddHandler(
                        PointerReleasedEvent,
                        OnBatteryReleased,
                        RoutingStrategies.Tunnel
                    );*/
                    /*var parent = (Control?)this.VisualRoot;
                    parent?.AddHandler(
                        Control.PointerMovedEvent,
                        OnParentMoved,
                        RoutingStrategies.Tunnel
                    );*/
                }
                else
                {
                    this.Classes.Remove("left-selected");
                }
            });
    }

    private bool _isDragCompleted = false;

    private void OnParentMoved(object? sender, PointerEventArgs e)
    {
        if (_isDragCompleted)
            return;
        AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
        if (adornerLayer != null)
        {
            var adorners = adornerLayer.Children;

            adornerLayer.Children.RemoveAll(adorners);

            DragAdorner _adorner = new DragAdorner(
                e.GetPosition((Control)sender),
                new Battery() { Classes = { "Left" } }
            );
            adornerLayer.Children.Add(_adorner);
        }
    }

    private async void OnBatteryMoved(object? sender, PointerEventArgs e)
    {
        _isDragCompleted = false;
        // 当拖拽操作开始时，在源列表中开始拖拽
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            DataObject dataObject = new DataObject();
            dataObject.Set("dataObject", this);
            DragDrop.DoDragDrop(e, dataObject, DragDropEffects.Move);
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
            if (adornerLayer != null)
            {
                var adorners = adornerLayer.Children;

                adornerLayer.Children.RemoveAll(adorners);
            }

            _isDragCompleted = true;
        }
    }

    public void OnBatteryReleased(object? sender, PointerReleasedEventArgs e) { }

    public void OnBatteryPressed(object? sender, PointerPressedEventArgs e)
    {
        var PART_LeftBattery = sender as Battery;
        if (e.GetCurrentPoint(PART_LeftBattery).Properties.IsLeftButtonPressed)
        {
            var data = new DataObject();
            data.Set("dataObject", PART_LeftBattery);
            DragDrop.DoDragDrop(e, data, DragDropEffects.Link);
        }
    }

    protected override void OnClick()
    {
        base.OnClick();
        IsSelected = true;
        SetPseudoclasses("isClicked", true);
    }

    private void SetPseudoclasses(string name, bool flag)
    {
        PseudoClasses.Set($":{name}", flag);
    }
}
