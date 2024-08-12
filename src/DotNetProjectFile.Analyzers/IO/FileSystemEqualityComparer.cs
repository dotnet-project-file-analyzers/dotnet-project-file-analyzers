using System.IO;

namespace DotNetProjectFile.IO;

internal sealed class FileSystemEqualityComparer<T> : IEqualityComparer<T>
    where T : FileSystemInfo
{
    public bool Equals(T x, T y) => x.FullName == y.FullName;

    public int GetHashCode(T obj) => obj.FullName.GetHashCode();
}
