namespace Rules.MS_Build.NuGet_packages.Define_package_info;

public class Reports_on_missing
{
    [Test]
    public void information_provided()
       => new DefinePackageInfo()
       .ForProject("NoPackageInfo.cs")
       .HasIssues(
           new Issue("Proj0201", "Define the <Version> or <VersionPrefix> node explicitly or define the <IsPackable> node with value 'false'."),
           new Issue("Proj0202", "Define the <Description> or <PackageDescription> node explicitly or define the <IsPackable> node with value 'false'."),
           new Issue("Proj0203", "Define the <Authors> node explicitly or define the <IsPackable> node with value 'false'."),
           new Issue("Proj0204", "Define the <PackageTags> node explicitly or define the <IsPackable> node with value 'false'."),
           new Issue("Proj0205", "Define the <RepositoryUrl> node explicitly or define the <IsPackable> node with value 'false'."),
           new Issue("Proj0206", "Define the <PackageProjectUrl> node explicitly or define the <IsPackable> node with value 'false'."),
           new Issue("Proj0207", "Define the <Copyright> node explicitly or define the <IsPackable> node with value 'false'."),
           new Issue("Proj0208", "Define the <PackageReleaseNotes> node explicitly or define the <IsPackable> node with value 'false'."),
           new Issue("Proj0209", "Define the <PackageReadmeFile> node explicitly or define the <IsPackable> node with value 'false'."),
           new Issue("Proj0210", "Define the <PackageLicenseExpression> or <PackageLicenseFile> node explicitly or define the <IsPackable> node with value 'false'."),
           new Issue("Proj0212", "Define the <PackageIcon> node explicitly or define the <IsPackable> node with value 'false'."),
           new Issue("Proj0213", "Define the <PackageIconUrl> node explicitly or define the <IsPackable> node with value 'false'."),
           new Issue("Proj0214", "Define the <PackageId> node explicitly or define the <IsPackable> node with value 'false'."),
           new Issue("Proj0216", "Define the <ProductName> node explicitly or define the <IsPackable> node with value 'false'."));

    [Test]
    public void version()
       => new DefinePackageInfo()
       .ForProject("NoVersion.cs")
       .HasIssue(
           new Issue("Proj0201", "Define the <Version> or <VersionPrefix> node explicitly or define the <IsPackable> node with value 'false'."));

    [Test]
    public void description()
       => new DefinePackageInfo()
       .ForProject("NoDescription.cs")
       .HasIssue(
           new Issue("Proj0202", "Define the <Description> or <PackageDescription> node explicitly or define the <IsPackable> node with value 'false'."));

    [Test]
    public void authors()
       => new DefinePackageInfo()
       .ForProject("NoAuthors.cs")
       .HasIssue(
           new Issue("Proj0203", "Define the <Authors> node explicitly or define the <IsPackable> node with value 'false'."));

    [Test]
    public void tags()
       => new DefinePackageInfo()
       .ForProject("NoTags.cs")
       .HasIssue(
           new Issue("Proj0204", "Define the <PackageTags> node explicitly or define the <IsPackable> node with value 'false'."));

    [Test]
    public void repository_url()
       => new DefinePackageInfo()
       .ForProject("NoRepositoryUrl.cs")
       .HasIssue(
           new Issue("Proj0205", "Define the <RepositoryUrl> node explicitly or define the <IsPackable> node with value 'false'."));

    [Test]
    public void url()
       => new DefinePackageInfo()
       .ForProject("NoUrl.cs")
       .HasIssue(
           new Issue("Proj0206", "Define the <PackageProjectUrl> node explicitly or define the <IsPackable> node with value 'false'."));

    [Test]
    public void copyright()
       => new DefinePackageInfo()
       .ForProject("NoCopyright.cs")
       .HasIssue(
           new Issue("Proj0207", "Define the <Copyright> node explicitly or define the <IsPackable> node with value 'false'."));

    [Test]
    public void release_notes()
       => new DefinePackageInfo()
       .ForProject("NoReleaseNotes.cs")
       .HasIssue(
           new Issue("Proj0208", "Define the <PackageReleaseNotes> node explicitly or define the <IsPackable> node with value 'false'."));

    [Test]
    public void readme()
       => new DefinePackageInfo()
       .ForProject("NoReadme.cs")
       .HasIssue(
           new Issue("Proj0209", "Define the <PackageReadmeFile> node explicitly or define the <IsPackable> node with value 'false'."));

    [Test]
    public void license()
       => new DefinePackageInfo()
       .ForProject("NoLicense.cs")
       .HasIssue(
           new Issue("Proj0210", "Define the <PackageLicenseExpression> or <PackageLicenseFile> node explicitly or define the <IsPackable> node with value 'false'."));

    [Test]
    public void icon()
       => new DefinePackageInfo()
       .ForProject("NoIcon.cs")
       .HasIssue(
           new Issue("Proj0212", "Define the <PackageIcon> node explicitly or define the <IsPackable> node with value 'false'."));

    [Test]
    public void icon_url()
       => new DefinePackageInfo()
       .ForProject("NoIconUrl.cs")
       .HasIssue(
           new Issue("Proj0213", "Define the <PackageIconUrl> node explicitly or define the <IsPackable> node with value 'false'."));

    [Test]
    public void product_name()
      => new DefinePackageInfo()
      .ForProject("NoProductName.cs")
      .HasIssue(
          new Issue("Proj0216", "Define the <ProductName> node explicitly or define the <IsPackable> node with value 'false'."));
}

public class Guards
{
    [Test]
    public void Test_project()
        => new DefinePackageInfo()
       .ForProject("TestProject.cs")
       .HasNoIssues();

    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    [TestCase("PackageDescription.cs")]
    [TestCase("WithLicenseFile.cs")]
    [TestCase("VersionPrefix.cs")]
    [TestCase("VersionPrefixAndSuffix.cs")]
    [TestCase("VersionPrefixAndSuffixAndVersion.cs")]
    public void Projects_without_issues(string project)
         => new DefinePackageInfo()
        .ForProject(project)
        .HasNoIssues();
}
