using DotNetProjectFile.MsBuild;

namespace MS_Build.Pragma_warning;

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
        => PragmaWarning.TryParse(str, None).Should().Be(new PragmaWarning("Proj0007", true, None));

    [TestCase("#pragma warning restore Proj0007")]
    public void Restore(string str)
        => PragmaWarning.TryParse(str, None).Should().Be(new PragmaWarning("Proj0007", false, None));
}
