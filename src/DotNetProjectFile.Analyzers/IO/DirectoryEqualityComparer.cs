using System.IO;

namespace DotNetProjectFile.IO;

internal sealed class DirectoryEqualityComparer : IEqualityComparer<DirectoryInfo>
{
    public static readonly DirectoryEqualityComparer Instance = new();

    public bool Equals(DirectoryInfo x, DirectoryInfo y) => x.FullName == y.FullName;

    public int GetHashCode(DirectoryInfo obj) => obj.FullName.GetHashCode();
}
