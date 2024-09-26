namespace Rules.MS_Build.NuGet_packages.Avoid_license_url;

public class Reports
{
    [Test]
    public void on_license_url()
       => new AvoidLicenseUrl()
       .ForProject("WithLicenseUrl.cs")
       .HasIssue(
            new Issue("Proj0211", "Replace deprecated <PackageLicenseUrl> with <PackageLicenseExpression> or <PackageLicenseFile> node.")
                .WithSpan(29, 4, 29, 101));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project)
         => new AvoidLicenseUrl()
        .ForProject(project)
        .HasNoIssues();
}
