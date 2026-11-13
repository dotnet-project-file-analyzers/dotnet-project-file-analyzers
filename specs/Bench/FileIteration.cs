#pragma warning disable RS1035 // Do not use APIs banned for analyzers

using DotNetProjectFile.IO;

namespace Bench;

public class FileIteration
{
    private const string Root = "C:\\code\\dotnet-project-file-analyzers";

    [Benchmark]
    public int IO_Files()
    {
        var count = 0;
        var files = new IOFiles(Root, IOFiles.DefaultIgnores);

        foreach (var file in files)
        {
            count += file.FullName.Length;
        }
        return count;
    }

    [Benchmark(Baseline = true)]
    public int IO_FileSystem()
    {
        var count = 0;
        var root = new DirectoryInfo(Root);

        foreach (var file in root
            .EnumerateFiles("*", SearchOption.AllDirectories))
        {
            count += file.FullName.Length;
        }

        return count;
    }
}
