namespace System.IO;

internal static class SystemFileSystemInfoExtensions
{
    public static IEnumerable<DirectoryInfo> GetAncestors(this FileInfo file)
    {
        var current = file.Directory;
        while (current is { })
        {
            yield return current;
            current = current.Parent;
        }
    }

    public static FileInfo File(this DirectoryInfo dir, string name) => new(Path.Combine(dir.FullName, name));
}
