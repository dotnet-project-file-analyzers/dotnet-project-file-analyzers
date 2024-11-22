using DotNetProjectFile.IO;

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
        => Glob.TryParse(str)
            .Should().NotBeNull()
            .And.Subject.ToString().Should().Be(str);
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
    [TestCase("{cs,vb}}")]
    public void globs(string str)
        => Glob.TryParse(str).Should().BeNull();
}
