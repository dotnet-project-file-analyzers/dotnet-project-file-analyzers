namespace Rules.MS_Build.Remove_redundant_references_for_globally_referenced;

public class Reports
{
    [Test]
    public void on_redundant_included() => new RemoveredundantReferencesForGloballyReferenced()
            .ForProject("RemoveRedundantReferencesForGloballyReferenced.cs")
            .HasIssue(
                Issue.WRN("Proj0812", "Remove redundant reference 'DotNetProjectFile.Analyzers'").WithSpan(07, 04, 07, 62));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void project_files_as_additional(string project) => new RemoveredundantReferencesForGloballyReferenced()
        .ForProject(project)
        .HasNoIssues();
}
