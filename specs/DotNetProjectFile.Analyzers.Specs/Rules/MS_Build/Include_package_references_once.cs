namespace Rules.MS_Build.Include_package_references_once;

public class Reports
{
    [Test]
    public void on_double_imports()
       => new IncludePackageReferencesOnce()
       .ForProject("DoublePackageReferences.cs")
       .HasIssues(
            new Issue("Proj0013", "Package 'Qowaiv' is already referenced.").WithSpan(05, 04, 05, 57),
            new Issue("Proj0013", "Package 'Qowaiv' is already referenced.").WithSpan(06, 04, 06, 56),
            new Issue("Proj0013", "Package 'Qowaiv' is already referenced.").WithSpan(10, 04, 10, 57),
            new Issue("Proj0013", "Package 'Qowaiv' is already referenced.").WithSpan(11, 04, 11, 57),
            new Issue("Proj0013", "Package 'Qowaiv' is already referenced.").WithSpan(13, 04, 13, 56));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project)
         => new IncludePackageReferencesOnce()
        .ForProject(project)
        .HasNoIssues();
}
