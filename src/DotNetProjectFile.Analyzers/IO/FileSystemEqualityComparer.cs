using System.IO;

namespace DotNetProjectFile.IO;

internal static class FileSystemEqualityComparer
{
    public static readonly FileSystemEqualityComparer<DirectoryInfo> Directory = new();
    public static readonly FileSystemEqualityComparer<FileInfo> File = new();
}

internal sealed class FileSystemEqualityComparer<T> : IEqualityComparer<T>
    where T : FileSystemInfo
{
    public bool Equals(T x, T y) => x.FullName == y.FullName;

    public int GetHashCode(T obj) => obj.FullName.GetHashCode();
}
