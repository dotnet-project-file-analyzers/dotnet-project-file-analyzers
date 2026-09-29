using DotNetProjectFile.IO;
using System.IO;

namespace Specs.TestTools;

public static class TestPath
{
    public static string Relative(string path) => IOFile.Parse(path).ToString();

    public static string Full(string path)
    {
        var dir = new DirectoryInfo("../../../../../projects");
        var combined = Path.Combine(dir.FullName, path);
        return IOFile.Parse(combined).ToString();
    }
}
