using System.IO;

namespace DotNetProjectFile.IO;

public static class FileSystem
{
    public static readonly IEqualityComparer<DirectoryInfo> DirectoryComparer = new FileSystemEqualityComparer<DirectoryInfo>();

    public static readonly IComparer<string?> PathCompare = new FilePathComparer();

}
