namespace Rules.MS_Build.Migrate_away_from_BinaryFormatter;

public class Reports
{
    [Test]
    public void when_BinaryFormatter_is_enabled() => new MigrateAwayFromBinaryFormatter()
        .ForProject("BinaryFormattingEnabled.cs")
        .HasIssue(Issue.WRN("Proj0032", "Migrate away from BinaryFormatter and disable <EnableUnsafeBinaryFormatterSerialization>.")
            .WithSpan(04, 04, 04, 93));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new MigrateAwayFromBinaryFormatter()
        .ForProject(project)
        .HasNoIssues();
}
