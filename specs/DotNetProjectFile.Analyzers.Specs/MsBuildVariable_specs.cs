using DotNetProjectFile.MsBuild;

namespace Specs.MsBuildVariable_specs;

public class Parsing
{
    [TestCase("   $(Trim)  ", "$(Trim)")]
    [TestCase("$(MyVariable)")]
    public void Text(string str, string? expected = null)
        => MsBuildVariable.TryParse(str).ToString().Should().Be(expected ?? str);


    [TestCase("$(One) Two $(Three) $(Four", "$(One)", "$(Three)")]
    public void All(string str, params string[] parsed)
        => MsBuildVariable.ParseAll(str).Select(v => v.ToString())
        .Should().BeEquivalentTo(parsed);
}
