namespace DotNetProjectFile.IO;

public sealed class FilePathComparer : IComparer<string?>
{
    /// <inheritdoc />
    public int Compare(string? x, string? y)
    {
        x ??= string.Empty;
        y ??= string.Empty;
        int? compare = null;
        var xs = x.GetEnumerator();
        var ys = y.GetEnumerator();

        while (compare is null && xs.MoveNext() && ys.MoveNext())
        {
            compare = Compare(xs.Current, ys.Current);
        }
        return compare ?? x.Length.CompareTo(y.Length);
    }

    private int? Compare(char x, char y)
    {
        int compare = Update(x).CompareTo(Update(y));
        return compare == 0 ? null : compare;

        static char Update(char ch)
            => ch == '/' || ch == '\\'
            ? char.MinValue
            : char.ToUpperInvariant(ch);
    }
}
