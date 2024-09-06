namespace Rules.MS_Build.NuGet_packages.Package_icon;

public class Reports
{
    [Test]
    public void Bitmap_Image()
        => new ProvideCompliantPackageIcon()
        .ForProject("BmpIcon.cs")
        .HasIssues(
            new Issue("Proj0215", "The package icon 'big-icon.bmp' is recommended to be a PNG.").WithSpan(07, 04, 07, 43),
            new Issue("Proj0215", "The package icon 'big-icon.bmp' must be less then 1MB."/*.*/).WithSpan(07, 04, 07, 43));

    [Test]
    public void Different_dimensions()
       => new ProvideCompliantPackageIcon()
       .ForProject("BigIcon.cs")
       .HasIssue(new Issue("Proj0215", "The package icon 'logo_400x400.png' is recommended to be 128x128 not 400x400.")
       .WithSpan(07, 04, 07, 47));
}
public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    [TestCase("PackageDescription.cs")]
    [TestCase("WithLicenseFile.cs")]
    public void Projects_without_issues(string project)
        => new DefinePackageInfo()
        .ForProject(project)
        .HasNoIssues();
}
