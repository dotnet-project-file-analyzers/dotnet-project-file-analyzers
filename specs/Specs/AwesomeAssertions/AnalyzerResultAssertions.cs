using Buildalyzer;

namespace AwesomeAssertions;

public sealed class AnalyzerResultAssertions(IAnalyzerResult subject)
{
    public IAnalyzerResult Subject { get; } = subject;

    public AndConstraint<AnalyzerResultAssertions> HaveProperties(Dictionary<string, string> expected)
    {
        var properties = Subject.Properties;

        properties.Should().Contain(expected);

        return new(this);
    }

    public AndConstraint<AnalyzerResultAssertions> HaveCompilerVisibleProperties(params string[] expected)
    {
        var items = (Subject.Items.TryGetValue("CompilerVisibleProperty", out var actual) ? actual : [])
           .Select(x => x.ItemSpec)
           .Order()
           .ToArray();

        items.Should().Contain(expected);

        return new(this);
    }

    public AndConstraint<AnalyzerResultAssertions> HaveItems(string name, params Specs.TestTools.ProjectItem[] expected)
    {
        var items = (Subject.Items.TryGetValue(name, out var actual) ? actual : [])
            .OrderBy(x => x.ItemSpec)
            .ToArray();

        items.Should().BeEquivalentTo(expected);

        return new(this);
    }
}
