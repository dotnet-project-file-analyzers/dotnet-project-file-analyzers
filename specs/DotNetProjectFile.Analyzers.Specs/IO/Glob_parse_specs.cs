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
    public void single(string str)
    {
        var glob = GlobParser.TryParse(str);

        glob!.ToString().Should().Be(str);
    }
}
