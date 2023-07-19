using DotNetProjectFile.IO;

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

    public static FileInfo SelectFile(this DirectoryInfo directory, string path)
        => new(FileSystem.Normalize(Path.Combine(directory.FullName, path)));

    public static DirectoryInfo SelectDirectory(this DirectoryInfo directory, string path)
        => new(FileSystem.Normalize(Path.Combine(directory.FullName, path)));
}
