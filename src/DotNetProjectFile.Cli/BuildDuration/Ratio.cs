using Qowaiv;

namespace DotNetProjectFile.Cli.BuildDuration;

public static class Ratio
{
    [Pure]
    public static Percentage Between(TimeSpan duration, TimeSpan total)
        => (Percentage)(duration.Ticks / (decimal)total.Ticks);
}
