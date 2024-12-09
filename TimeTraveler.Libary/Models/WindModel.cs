using CommunityToolkit.Mvvm.ComponentModel;
using TimeTraveler.Libary.Definitions;

namespace TimeTraveler.Libary.Models;

public partial class WindModel : ObservableObject
{
    public int Id { get; set; }

    public string Name => ToString();

    public bool IsHasElement { get; set; }
    public WindDirection Direction { get; set; }

    public override string ToString()
    {
        if (Direction is WindDirection windDirection)
        {
            switch (windDirection)
            {
                case WindDirection.None:
                    return "静止";
                case WindDirection.WindClockwiseFastly:
                    return "东北风";
                case WindDirection.WindAnticlockwiseFastly:
                    return "西南风";
                case WindDirection.WindClockwiseSlowly:
                    return "北风";
                case WindDirection.WindAnticlockwiseSlowly:
                    return "西风";
                case WindDirection.WindCompletelyStopped:
                    return "东风";
            }
        }
        return "未知风向";
    }
}
