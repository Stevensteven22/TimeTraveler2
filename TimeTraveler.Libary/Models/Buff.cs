namespace TimeTraveler.Libary.Models;

[SQLite.Table("buff")]
public class Buff
{
    [SQLite.PrimaryKey, SQLite.AutoIncrement]
    [SQLite.Column("id")]
    public int Id { get; set; } = -1;

    [SQLite.Column("name")]
    public string Name { get; set; }

    //属性的实际百分比（不是加成）
    [SQLite.Column("value1")]
    public double Value1 { get; set; }
    
    //属性的实际数值（不是加成）
    [SQLite.Column("value2")]
    public double Value2 { get; set; }

    [SQLite.Column("description")]
    public string Description { get; set; }

    [SQLite.Column("createdAt")]
    public DateTime CreatedAt { get; set; }

    [SQLite.Column("updatedAt")]
    public DateTime UpdatedAt { get; set; }
}
