namespace Rules.MS_Build.Include_package_references_once;

public class Reports
{
    [Test]
    public void on_double_imports()
       => new IncludePackageReferencesOnce()
       .ForProject("DoublePackageReferences.cs")
       .HasIssues(
            new Issue("Proj0013", "Package 'Qowaiv' is already referenced.").WithSpan(04, 04, 04, 57).WithPath("DoublePackageReferences.props"),
            new Issue("Proj0013", "Package 'Qowaiv' is already referenced.").WithSpan(04, 04, 04, 57).WithPath("DoublePackageReferences.csproj"),
            new Issue("Proj0013", "Package 'Qowaiv' is already referenced.").WithSpan(10, 04, 10, 57).WithPath("DoublePackageReferences.csproj"),
            new Issue("Proj0013", "Package 'Qowaiv' is already referenced.").WithSpan(11, 04, 11, 57).WithPath("DoublePackageReferences.csproj"));
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
