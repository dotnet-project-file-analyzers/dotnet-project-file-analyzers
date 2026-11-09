using System.Collections.Generic;

namespace DotNetProjectFile.Cli;

public static class Durations
{
    extension(TimeSpan duration)
    {
        /// <summary>Parses a number of seconds, like <c>52.361</c>.</summary>
        /// <remarks>
        /// Less than a value, like <c>&lt;0.001</c>, is taken as half of that value.
        /// </remarks>
        [Pure]
        public static TimeSpan ParseSeconds(string seconds)
            => seconds.StartsWith('<')
            ? TimeSpan.FromSeconds(double.Parse(seconds[1..], CultureInfo.InvariantCulture) / 2)
            : TimeSpan.FromSeconds(double.Parse(seconds, CultureInfo.InvariantCulture));

        [Pure]
        public string Clock()
            => duration.ToString(duration.TotalHours >= 1 ? @"h\:mm\:ss" : @"mm\:ss", CultureInfo.InvariantCulture);

        [Pure]
        public string Short()
            => duration.TotalMinutes >= 1
            ? invariant($"{(int)duration.TotalMinutes}m{duration.Seconds:00}s")
            : invariant($"{duration.Seconds}s");

        public string Format()
        {
            var round = Math.Round((decimal)duration.TotalSeconds, 3);

            return round == 0
                ? "<0.001 s"
                : invariant($"{round:#,##0.000} s");
        }
    }

    extension(IEnumerable<TimeSpan> durations)
    {
        /// <summary>Computes the sum of the durations.</summary>
        [Pure]
        public TimeSpan Sum() => TimeSpan.FromTicks(durations.Sum(d => d.Ticks));
    }
}
