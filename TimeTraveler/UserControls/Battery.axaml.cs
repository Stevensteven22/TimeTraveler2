using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using System;
using Avalonia.Controls.Metadata;

namespace TimeTraveler.UserControls;

[PseudoClasses( ":isClicked")]
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
        this.GetObservable(IsLeftSelectedProperty).Subscribe(newValue =>
        {
            if (newValue)
            {
                this.Classes.Add("left-selected");
            }
            else
            {
                this.Classes.Remove("left-selected");
            }
        });
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
