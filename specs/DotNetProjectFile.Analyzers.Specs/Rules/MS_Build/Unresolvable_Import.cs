using DotNetProjectFile.MsBuild;

namespace Rules.MS_Build.Unresolvable_Import;

public class Reports
{
    [Test]
    public void empty_includes() => new UnresolvableImport()
    .ForProject("UnresolvableImport.cs")
        .HasIssue(Issue.WRN("Proj0034", "The <Import> '$(MSBuildThisFileDirectory)common.props' could not be resolved by the analyzer.").WithSpan(02, 02, 02, 62));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_with_analyzers(string project) => new UnresolvableImport()
        .ForProject(project)
        .HasNoIssues();
}
