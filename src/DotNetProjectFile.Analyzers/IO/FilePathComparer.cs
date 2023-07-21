namespace DotNetProjectFile.IO;

public sealed class FilePathComparer : IComparer<string>
{
    /// <inheritdoc />
    public int Compare(string x, string y)
    {
        int? compare = null;
        var xs = x.GetEnumerator();
        var ys = y.GetEnumerator();

        while (compare is null && xs.MoveNext() && ys.MoveNext())
        {
            compare = Compare(xs.Current, ys.Current);
        }
        return compare ?? y.Length.CompareTo(x.Length);
    }

    private int? Compare(char x, char y)
    {
        int compare = Update(x).CompareTo(Update(y));
        return compare == 0 ? null : compare;

        static char Update(char ch) => ch == '/' || ch == '\\' ? char.MinValue : ch;
    }
}
