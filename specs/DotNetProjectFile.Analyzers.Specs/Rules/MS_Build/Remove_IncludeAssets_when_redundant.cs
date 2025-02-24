namespace Rules.MS_Build.Remove_IncludeAssets_when_redundant;

public class Reports
{
    [Test]
    public void redundant_includes()
       => new RemoveIncludeAssetsWhenRedundant()
       .ForProject("IncludeAssets.cs")
       .HasIssues(
           Issue.WRN("Proj0026", "Remove <IncludeAssets> as it is redundant when all assets are private" /*.*/).WithSpan(20, 04, 23, 23),
           Issue.WRN("Proj0026", "Remove IncludeAssets as it is redundant when all assets are private" /*...*/).WithSpan(24, 04, 24, 163));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project)
         => new RemoveIncludeAssetsWhenRedundant()
        .ForProject(project)
        .HasNoIssues();
}
