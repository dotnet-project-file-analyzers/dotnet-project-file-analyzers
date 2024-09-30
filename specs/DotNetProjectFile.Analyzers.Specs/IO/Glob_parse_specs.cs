using DotNetProjectFile.IO.Globbing;

namespace IO.Glob_specs;

public class Parses
{
    [TestCase("?")]
    [TestCase("*")]
    [TestCase("**")]
    [TestCase("*.*")]
    [TestCase("[Hh]ello.*")]
    [TestCase("[!H]ello.*")]
    [TestCase("*.{cs,vb,csproj}")]
    [TestCase("*.{cs,{vb,vpproj},csproj}")]
    public void globs(string str)
    {
        var glob = GlobParser.TryParse(str);

        glob!.ToString().Should().Be(str);
    }
}
