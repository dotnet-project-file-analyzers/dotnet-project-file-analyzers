namespace Benchmarks.Glob_checks;

public class GitIgnoreFiles
{
    [Test]
    public void Parse_050_lines()
    {
        var benchmarks = new GitIgnoreFile("../../../../../specs/Benchmarks/Files/") { Index = 0 };
        var parsed = benchmarks.Parse();

        parsed.Patterns.Should().HaveCount(39);

        parsed.Patterns.Should().AllSatisfy(p => p.Pattern.Should().NotBeNull());
    }

    [Test]
    public void Parse_231_lines()
    {
        var benchmarks = new GitIgnoreFile("../../../../../specs/Benchmarks/Files/") { Index = 1 };
        var parsed = benchmarks.Parse();

        parsed.Patterns.Should().HaveCount(129);

        parsed.Patterns.Should().AllSatisfy(p => p.Pattern.Should().NotBeNull());
    }
}
