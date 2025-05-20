namespace Rules.MS_Build.Enable_restore_locked_mode;

public class Reports
{
    [Test]
    public void on_missing() => new EnableRestoreLockedMode()
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <RestorePackagesWithLockFile>true</RestorePackagesWithLockFile>
              </PropertyGroup>
            </Project>")
        .HasIssue(
            Issue.WRN("Proj0044", "Define the <RestoreLockedMode> node with value 'true' when <ContinuousIntegrationBuild> is enabled").WithSpan(0, 0, 0, 32));

    [Test]
    public void on_false() => new EnableRestoreLockedMode()
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <RestorePackagesWithLockFile>true</RestorePackagesWithLockFile>
                <RestoreLockedMode>false</RestoreLockedMode>
              </PropertyGroup>
            </Project>")
        .HasIssue(
            Issue.WRN("Proj0044", "Define the <RestoreLockedMode> node with value 'true' when <ContinuousIntegrationBuild> is enabled").WithSpan(4, 16, 4, 60));

    [Test]
    public void on_unconditially_true() => new EnableRestoreLockedMode()
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <RestorePackagesWithLockFile>true</RestorePackagesWithLockFile>
                <RestoreLockedMode>true</RestoreLockedMode>
              </PropertyGroup>
            </Project>")
        .HasIssue(
            Issue.WRN("Proj0044", "Define the <RestoreLockedMode> node with value 'true' when <ContinuousIntegrationBuild> is enabled").WithSpan(4, 16, 4, 59));

    [Test]
    public void on_conditionally_level_1_false() => new EnableRestoreLockedMode()
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <RestorePackagesWithLockFile>true</RestorePackagesWithLockFile>
              </PropertyGroup>

              <PropertyGroup Condition=""'$(ContinuousIntegrationBuild)' == 'true'"">
                <RestoreLockedMode>false</RestoreLockedMode>
              </PropertyGroup>
            </Project>")
        .HasIssue(
            Issue.WRN("Proj0044", "Define the <RestoreLockedMode> node with value 'true' when <ContinuousIntegrationBuild> is enabled").WithSpan(7, 16, 7, 60));

    [Test]
    public void on_conditionally_level_2_false() => new EnableRestoreLockedMode()
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <RestorePackagesWithLockFile>true</RestorePackagesWithLockFile>
                <RestoreLockedMode Condition=""'$(ContinuousIntegrationBuild)' == 'true'"">false</RestoreLockedMode>
              </PropertyGroup>
            </Project>")
        .HasIssue(
            Issue.WRN("Proj0044", "Define the <RestoreLockedMode> node with value 'true' when <ContinuousIntegrationBuild> is enabled").WithSpan(4, 16, 4, 114));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void compliant(string project) => new EnableRestoreLockedMode()
        .ForProject(project)
        .HasNoIssues();

    [Test]
    public void on_lock_files_undefined() => new EnableRestoreLockedMode()
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
              </PropertyGroup>
            </Project>")
        .HasNoIssues();

    [Test]
    public void on_lock_files_disabled() => new EnableRestoreLockedMode()
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <RestorePackagesWithLockFile>false</RestorePackagesWithLockFile>
              </PropertyGroup>
            </Project>")
        .HasNoIssues();

    [Test]
    public void on_conditionally_level_1_true() => new EnableRestoreLockedMode()
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <RestorePackagesWithLockFile>true</RestorePackagesWithLockFile>
              </PropertyGroup>

              <PropertyGroup Condition=""'$(ContinuousIntegrationBuild)' == 'true'"">
                <RestoreLockedMode>true</RestoreLockedMode>
              </PropertyGroup>
            </Project>")
        .HasNoIssues();

    [Test]
    public void on_conditionally_level_2_true() => new EnableRestoreLockedMode()
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <RestorePackagesWithLockFile>true</RestorePackagesWithLockFile>
                <RestoreLockedMode Condition=""'$(ContinuousIntegrationBuild)' == 'true'"">true</RestoreLockedMode>
              </PropertyGroup>
            </Project>")
        .HasNoIssues();
}
