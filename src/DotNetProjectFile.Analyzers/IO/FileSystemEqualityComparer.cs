using System.IO;

namespace DotNetProjectFile.IO;

internal sealed class FileSystemEqualityComparer<T> : IEqualityComparer<T>
    where T : FileSystemInfo
{
    public bool Equals(T x, T y) => FileSystem.Normalize(x.FullName) == FileSystem.Normalize(y.FullName);

    public int GetHashCode(T obj) => FileSystem.Normalize(obj.FullName).GetHashCode();
}
