namespace DotNetProjectFile.IO;

public static class FileSystem
{
    public static readonly IComparer<string?> PathCompare = new FilePathComparer();
}
