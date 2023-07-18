namespace Rules.MS_Build.Include_project_references_once;

public class Reports
{
    [Test]
    public void on_double_imports()
       => new IncludeProjectReferencesOnce()
       .ForProject("DoubleProjectReferences.cs")
       .HasIssues(
            new Issue("Proj0014", @"Project '.\..\..\Projects\EmptyNodes\EmptyNodes.csproj' is already referenced.").WithSpan(11, 5, 11, 80));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project)
         => new IncludeProjectReferencesOnce()
        .ForProject(project)
        .HasNoIssues();
}
