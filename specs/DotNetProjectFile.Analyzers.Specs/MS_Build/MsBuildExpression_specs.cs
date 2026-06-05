using DotNetProjectFile.MsBuild;

namespace Specs.MsBuild.MsBuildExprssion_specs;

public class Parsing
{
    [TestCase("   $(Trim)  ", "$(Trim)")]
    [TestCase("$(With a $ insight)")]
    [TestCase("$(MyVariable)")]
    public void Text(string str, string? expected = null)
        => MsBuildExpression.TryParse(str).ToString().Should().Be(expected ?? str);


    [TestCase("$(One) Two $(Three) $(Four", "$(One)", "$(Three)")]
    public void All(string str, params string[] parsed)
        => MsBuildExpression.ParseAll(str).Select(v => v.ToString())
        .Should().BeEquivalentTo(parsed);

    [TestCase("$")]
    [TestCase("$$(Double)")]
    [TestCase("$(NoClosing")]
    [TestCase("(NoClosingDolar)")]
    public void Fails(string str)
        => MsBuildExpression.ParseAll(str).Should().BeEmpty();
}
