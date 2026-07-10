using Buildalyzer;
using System.Diagnostics;

namespace AwesomeAssertions;

public sealed class AnalyzerResultAssertions(IAnalyzerResult subject)
{
    public IAnalyzerResult Subject { get; } = subject;

    [DebuggerStepThrough]
    public AndConstraint<AnalyzerResultAssertions> HaveProperties(Dictionary<string, string> expected)
    {
        var properties = Subject.Properties;

        properties.Should().Contain(expected);

        return new(this);
    }

    [DebuggerStepThrough]
    public AndConstraint<AnalyzerResultAssertions> HaveCompilerVisibleProperties(params string[] expected)
    {
        var items = (Subject.Items.TryGetValue("CompilerVisibleProperty", out var actual) ? actual : [])
           .Select(x => x.ItemSpec)
           .Order()
           .ToArray();

        items.Should().Contain(expected);

        return new(this);
    }

    [DebuggerStepThrough]
    public AndConstraint<AnalyzerResultAssertions> HaveAdditionalFiles(params Specs.TestTools.ProjectItem[] expected)
        => HaveItems("AdditionalFiles", expected);

    [DebuggerStepThrough]
    public AndConstraint<AnalyzerResultAssertions> HaveItems(string name, params Specs.TestTools.ProjectItem[] expected)
    {
        var items = Subject.Items.OfType(name).ToArray();

        items.Should().BeEquivalentTo(expected);

        return new(this);
    }
}
