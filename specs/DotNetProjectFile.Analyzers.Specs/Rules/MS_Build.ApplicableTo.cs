namespace Rules.MS_Build;

public class ApplicableTo
{
    [TestCaseSource(nameof(Analyzers))]
    public void Not_empty(MsBuildProjectFileAnalyzer analyzer)
        => analyzer.ApplicableTo.Should().NotBeEmpty();

    private static readonly IEnumerable<MsBuildProjectFileAnalyzer> Analyzers = AvailableAnalyzers.MS_Build;
}
