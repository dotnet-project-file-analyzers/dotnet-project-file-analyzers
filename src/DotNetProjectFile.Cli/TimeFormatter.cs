namespace DotNetProjectFile.Cli;

public static class TimeFormatter
{
    extension(TimeSpan duration)
    {
        public string Format()
        {
            var round = Math.Round((decimal)duration.TotalSeconds, 3);

            return round == 0
                ? "<0.001 s"
                : invariant($"{round:#,##0.000} s");
        }
    }
}
