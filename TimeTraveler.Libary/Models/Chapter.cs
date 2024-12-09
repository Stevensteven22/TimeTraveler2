using CommunityToolkit.Mvvm.ComponentModel;

namespace TimeTraveler.Libary.Models;

[SQLite.Table("chapter")]
public class Chapter : ObservableObject
{
    [SQLite.Column("id")]
    public int Id { get; set; }

    [SQLite.Column("name")]
    public string Name { get; set; } = string.Empty;

    private bool _isSelected;

    [SQLite.Column("isSelected")]
    public virtual bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }

    private bool _isOK;

    [SQLite.Column("isOK")]
    public virtual bool IsOK
    {
        get => _isOK;
        set => SetProperty(ref _isOK, value);
    }

    private DateTime _updatedTime;

    [SQLite.Column("updatedTime")]
    public virtual DateTime UpdatedTime
    {
        get => _updatedTime;
        set => SetProperty(ref _updatedTime, value);
    }

    private bool _isEnabled;

    [SQLite.Ignore]
    public bool IsEnabled
    {
        get => _isEnabled;
        set => SetProperty(ref _isEnabled, value);
    }
}
