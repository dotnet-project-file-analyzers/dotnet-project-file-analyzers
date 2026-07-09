namespace Rules.MS_Build.Include_project_references_once;

public class Reports
{
    [Test]
    public void on_double_imports()
       => new IncludeProjectReferencesOnce()
       .ForProject("DoubleProjectReferences.cs")
       .HasIssues(
            Issue.WRN("Proj0014", @"Project './../../projects/EmptyNodes/EmptyNodes.csproj' is already referenced").WithSpan(11, 04, 11, 80));
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

public class Reports_across_files
{
    [Test]
    public void Same_project_referenced_in_csproj_and_in_imported_props_is_flagged_as_duplicate()
        => new IncludeProjectReferencesOnce()
            .ForProject("DuplicateProjectReferenceAcrossFiles.cs")
            .HasIssue(
                Issue.WRN("Proj0014", "Project '../EmptyNodes/EmptyNodes.csproj' is already referenced").WithSpan(07, 04, 07, 66));
}
