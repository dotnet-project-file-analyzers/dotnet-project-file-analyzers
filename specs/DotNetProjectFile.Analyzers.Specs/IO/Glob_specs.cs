using DotNetProjectFile.IO;

namespace IO.Glob_specs;

public class Parses
{
    [TestCase(".", "\\.")]
    [TestCase("?", ".")]
    [TestCase("*", "[^/]*")]
    [TestCase("**", ".*")]
    [TestCase("hello world", "hello world")]
    [TestCase("*.cs", "[^/]*\\.cs")]
    [TestCase("[^a]", "[\\^a]")]
    [TestCase("test[ab].cs", "test[ab]\\.cs")]
    [TestCase("test[.ab].cs", "test[\\.ab]\\.cs")]
    [TestCase("test[a.b].cs", "test[a\\.b]\\.cs")]
    [TestCase("test[!ab].cs", "test[^ab]\\.cs")]
    [TestCase("**.{cs,vb}", ".*\\.(cs|vb)")]
    public void expressions(string expression, string pattern)
    {
        var glob = Glob.TryParse(expression)!;
        glob.Should().NotBeNull();
        glob.Pattern.ToString().Should().Be(pattern);
    }
}

public class Does_not_parse
{
    [TestCase("test\\file.gif", "Only forward slashes can be used.")]
    [TestCase("***", "Not more then 2 starts in a row")]
    [TestCase("[]", "Sequence should contain at least on character")]
    [TestCase("[!]", "Not sequence should contain at least on character")]
    public void expressions(string expression, string because)
    {
        var glob = Glob.TryParse(expression)!;
        glob.Should().BeNull(because);
    }
}
