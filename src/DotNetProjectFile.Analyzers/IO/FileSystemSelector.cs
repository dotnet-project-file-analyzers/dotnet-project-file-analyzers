using System.Diagnostics.Contracts;
using System.IO;

namespace DotNetProjectFile.IO;

public static class FileSystemSelector
{
    [Pure]
    public static IEnumerable<FileInfo>? Files(this DirectoryInfo directory, string path)
    {
        try
        {
            return Iterate(directory, path);
        }
        catch
        {
            return null;
        }
    }

    [Pure]
    private static IEnumerable<FileInfo>? Iterate(DirectoryInfo directory, string path)
    {
        // We do not support variables yet.
        if (path.Contains("$(")) return null;

        IEnumerable<DirectoryInfo> enumerator = new RootDirectory(directory);
        var parts = path.Split('/', '\\');

        foreach (var part in parts.Take(parts.Length - 1))
        {
            if (part == ".")
            {
                // keep current.
            }
            else if (part == "..")
            {
                if (enumerator is RootDirectory root)
                {
                    enumerator = root.Parent;
                }
                else
                {
                    enumerator = enumerator.Select(d => d.Parent!);
                }
            }
            else if (part == "**")
            {
                enumerator = enumerator.SelectMany(d => d.EnumerateDirectories("*", SearchOption.AllDirectories));
            }
            else
            {
                enumerator = enumerator.SelectMany(d => d.EnumerateDirectories(part, SearchOption.TopDirectoryOnly));
            }
        }
        return enumerator
            .SelectMany(d => d.EnumerateFiles(parts[^1]));
    }

    private sealed class RootDirectory(DirectoryInfo root) : IEnumerable<DirectoryInfo>
    {
        private readonly DirectoryInfo Root = root;

        public RootDirectory Parent => new(Root.Parent);

        [Pure]
        public IEnumerator<DirectoryInfo> GetEnumerator() { yield return Root; }

        [Pure]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
