namespace Rules.MS_Build.Use_lock_files;

public class Reports
{
    [Test]
    public void on_missing() => new UseLockFiles()
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
              </PropertyGroup>
            </Project>")
        .HasIssue(
            Issue.WRN("Proj0043", "Define the <RestorePackagesWithLockFile> node with value 'true'").WithSpan(0, 0, 0, 0));

    [Test]
    public void on_false() => new UseLockFiles()
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <RestorePackagesWithLockFile>false</RestorePackagesWithLockFile>
              </PropertyGroup>
            </Project>")
        .HasIssue(
            Issue.WRN("Proj0043", "Define the <RestorePackagesWithLockFile> node with value 'true'").WithSpan(3, 16, 3, 80));

    [Test]
    public void on_empty() => new UseLockFiles()
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <RestorePackagesWithLockFile></RestorePackagesWithLockFile>
              </PropertyGroup>
            </Project>")
        .HasIssue(
            Issue.WRN("Proj0043", "Define the <RestorePackagesWithLockFile> node with value 'true'").WithSpan(3, 16, 3, 75));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void compliant(string project) => new UseLockFiles()
        .ForProject(project)
        .HasNoIssues();

    [Test]
    public void on_true() => new UseLockFiles()
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <RestorePackagesWithLockFile>true</RestorePackagesWithLockFile>
              </PropertyGroup>
            </Project>")
        .HasNoIssues();
}
