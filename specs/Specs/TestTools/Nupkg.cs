using System.IO;
using System.IO.Compression;

namespace Specs.TestTools;

public static class Nupkg
{
    public static IEnumerable<string> Read(FileInfo file)
    {
        using var archive = ZipFile.OpenRead(file.FullName);
        return archive.Entries.Select(e => e.FullName).Where(e => !e.EndsWith(".psmdcp"));
    }
}
