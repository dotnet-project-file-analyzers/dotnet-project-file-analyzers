using DotNetProjectFile.MsBuild;

namespace MS_Build.Pragma_warning;

public class Parses
{
    [TestCase("#pragma warning disable Proj0007")]
    [TestCase("#pragma warning disable Proj0007 ")]
    [TestCase("#pragma warning disable Proj0007\n")]
    [TestCase("#pragma warning disable Proj0007\t")]
    [TestCase("#pragma warning disable Proj0007\r\n")]
    [TestCase("#pragma warning disable Proj0007 with some ignored text afterwards")]
    [TestCase("  #pragma   warning   disable   Proj0007")]
    public void Disable(string str)
        => PragmaWarning.TryParse(str).Should().Be(new PragmaWarning("Project0007", true));

    [TestCase("#pragma warning restore Proj0007")]
    public void Restore(string str)
        => PragmaWarning.TryParse(str).Should().Be(new PragmaWarning("Project0007", false));
}
