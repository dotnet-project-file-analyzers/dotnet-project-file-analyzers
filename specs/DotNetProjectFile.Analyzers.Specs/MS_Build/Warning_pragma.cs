using DotNetProjectFile.CodeAnalysis;

namespace MS_Build.Warning_pragma;

public class Parses
{
    private static readonly Location None = Location.None;

    [TestCase("#pragma warning disable Proj0007")]
    [TestCase("#pragma warning disable Proj0007 ")]
    [TestCase("#pragma warning disable Proj0007\n")]
    [TestCase("#pragma warning disable Proj0007\t")]
    [TestCase("#pragma warning disable Proj0007\r\n")]
    [TestCase("#pragma warning disable Proj0007 with some ignored text afterwards")]
    [TestCase("  #pragma   warning   disable   Proj0007")]
    [TestCase("\r\n    #pragma   warning   disable   Proj0007")]
    public void Disable(string str)
        => WarningPragma.TryParse(str, None).Should().Be(new WarningPragma("Proj0007", true, None));

    [TestCase("#pragma warning restore Proj0007")]
    public void Restore(string str)
        => WarningPragma.TryParse(str, None).Should().Be(new WarningPragma("Proj0007", false, None));
}
