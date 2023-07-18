using System.IO;

namespace DotNetProjectFile.IO;

public static class FileSystem
{
    public static readonly IEqualityComparer<DirectoryInfo> DirectoryComparer = new FileSystemEqualityComparer<DirectoryInfo>();
    public static readonly IEqualityComparer<FileInfo> FileComparer = new FileSystemEqualityComparer<FileInfo>();
    public static readonly IComparer<string?> PathCompare = new FilePathComparer();

    public static string Normalize(string? path) => (path ?? string.Empty).Replace('\\', '/');
}
