namespace Rules.MS_Build.Remove_redundent_references_for_globally_referenced;

public class Reports
{
    [Test]
    public void on_redundent_included() => new RemoveRedundentReferencesForGloballyReferenced()
            .ForProject("RemoveRedundentReferencesForGloballyReferenced.cs")
            .HasIssue(
                Issue.WRN("Proj0812", "Remove redundent reference 'DotNetProjectFile.Analyzers'").WithSpan(07, 04, 07, 62));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void project_files_as_additional(string project) => new RemoveRedundentReferencesForGloballyReferenced()
        .ForProject(project)
        .HasNoIssues();
}
