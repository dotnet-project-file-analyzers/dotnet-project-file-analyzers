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
        glob.IsParseble.Should().BeTrue();
        glob.ToString().Should().Be(str);
    }
}

public class Can_not_parse
{
    [TestCase(",")]
    [TestCase("[")]
    [TestCase("{}")]
    [TestCase("{}}")]
    [TestCase("[[test]]")]
    [TestCase("[]")]
    [TestCase("*.{cs,{vb,vpproj}csproj}")]
    public void globs(string str)
    {
        var glob = GlobParser.TryParse(str);
        glob.IsParseble.Should().BeFalse();
    }
}
