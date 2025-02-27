using DotNetProjectFile.Text;

namespace Text.Glob_specs;

public class Matches
{
    [TestCase("?", "X")]
    [TestCase("*", "Hello")]
    [TestCase("**", "docs/test.md")]
    [TestCase("Hello", "Hello")]
    [TestCase("[He]", "H")]
    [TestCase("[He]", "e")]
    [TestCase("{cs,vbproj,csproj}", "cs")]
    [TestCase("{cs,vbproj,csproj}", "vbproj")]
    [TestCase("{cs,vbproj,csproj}", "csproj")]
    public void globs(Glob glob, string value)
        => glob.IsMatch(value).Should().BeTrue();
}

public class Does_not_matches
{
    [TestCase("?", "12")]
    [TestCase("*", "docs/test.md")]
    [TestCase("Hello", "hello")]
    [TestCase("Hello", "Hello girl")]
    [TestCase("[He]", "h")]
    [TestCase("[He]", "E")]
    [TestCase("[He]", "z")]
    [TestCase("{cs,vbproj,csproj}", "md")]
    public void globs(Glob glob, string value)
        => glob.IsMatch(value).Should().BeFalse();
}

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
