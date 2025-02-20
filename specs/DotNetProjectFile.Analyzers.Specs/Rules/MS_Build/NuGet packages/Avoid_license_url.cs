namespace Rules.MS_Build.NuGet_packages.Avoid_license_url;

public class Reports
{
    [Test]
    public void on_license_url()
       => new AvoidLicenseUrl()
       .ForProject("WithLicenseUrl.cs")
       .HasIssue(
            Issue.WRN("Proj0211", "Replace deprecated <PackageLicenseUrl> with <PackageLicenseExpression> or <PackageLicenseFile> node.")
                .WithSpan(30, 4, 30, 101));
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
