using DotNetProjectFile.BuildAgents;

namespace Rules.MS_Build.Enable_restore_locked_mode;

[NonParallelizable]
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

    [TestCase(BuildAgent.AnyCI, "TF_BUILD", true)]
    [TestCase(BuildAgent.AnyCI, "GITHUB_ACTIONS", true)]
    [TestCase(BuildAgent.AnyCI, "APPVEYOR", true)]
    [TestCase(BuildAgent.AnyCI, "TRAVIS", true)]
    [TestCase(BuildAgent.AnyCI, "CIRCLECI", true)]
    [TestCase(BuildAgent.AnyCI, "CODEBUILD_BUILD_ID", false)]
    [TestCase(BuildAgent.AnyCI, "BUILD_ID", false)]
    [TestCase(BuildAgent.AnyCI, "TEAMCITY_VERSION", false)]
    [TestCase(BuildAgent.AnyCI, "JB_SPACE_API_URL", false)]
    [TestCase(BuildAgent.AzurePipelines, "CI", true)]
    [TestCase(BuildAgent.AzurePipelines, "GITHUB_ACTIONS", true)]
    [TestCase(BuildAgent.AzurePipelines, "APPVEYOR", true)]
    [TestCase(BuildAgent.AzurePipelines, "TRAVIS", true)]
    [TestCase(BuildAgent.AzurePipelines, "CIRCLECI", true)]
    [TestCase(BuildAgent.AzurePipelines, "CODEBUILD_BUILD_ID", false)]
    [TestCase(BuildAgent.AzurePipelines, "BUILD_ID", false)]
    [TestCase(BuildAgent.AzurePipelines, "TEAMCITY_VERSION", false)]
    [TestCase(BuildAgent.AzurePipelines, "JB_SPACE_API_URL", false)]
    [TestCase(BuildAgent.GitHubActions, "CI", true)]
    [TestCase(BuildAgent.GitHubActions, "TF_BUILD", true)]
    [TestCase(BuildAgent.GitHubActions, "APPVEYOR", true)]
    [TestCase(BuildAgent.GitHubActions, "TRAVIS", true)]
    [TestCase(BuildAgent.GitHubActions, "CIRCLECI", true)]
    [TestCase(BuildAgent.GitHubActions, "CODEBUILD_BUILD_ID", false)]
    [TestCase(BuildAgent.GitHubActions, "BUILD_ID", false)]
    [TestCase(BuildAgent.GitHubActions, "TEAMCITY_VERSION", false)]
    [TestCase(BuildAgent.GitHubActions, "JB_SPACE_API_URL", false)]
    [TestCase(BuildAgent.AppVeyor, "CI", true)]
    [TestCase(BuildAgent.AppVeyor, "TF_BUILD", true)]
    [TestCase(BuildAgent.AppVeyor, "GITHUB_ACTIONS", true)]
    [TestCase(BuildAgent.AppVeyor, "TRAVIS", true)]
    [TestCase(BuildAgent.AppVeyor, "CIRCLECI", true)]
    [TestCase(BuildAgent.AppVeyor, "CODEBUILD_BUILD_ID", false)]
    [TestCase(BuildAgent.AppVeyor, "BUILD_ID", false)]
    [TestCase(BuildAgent.AppVeyor, "TEAMCITY_VERSION", false)]
    [TestCase(BuildAgent.AppVeyor, "JB_SPACE_API_URL", false)]
    [TestCase(BuildAgent.TravisCI, "CI", true)]
    [TestCase(BuildAgent.TravisCI, "TF_BUILD", true)]
    [TestCase(BuildAgent.TravisCI, "GITHUB_ACTIONS", true)]
    [TestCase(BuildAgent.TravisCI, "APPVEYOR", true)]
    [TestCase(BuildAgent.TravisCI, "CIRCLECI", true)]
    [TestCase(BuildAgent.TravisCI, "CODEBUILD_BUILD_ID", false)]
    [TestCase(BuildAgent.TravisCI, "BUILD_ID", false)]
    [TestCase(BuildAgent.TravisCI, "TEAMCITY_VERSION", false)]
    [TestCase(BuildAgent.TravisCI, "JB_SPACE_API_URL", false)]
    [TestCase(BuildAgent.CircleCI, "CI", true)]
    [TestCase(BuildAgent.CircleCI, "TF_BUILD", true)]
    [TestCase(BuildAgent.CircleCI, "GITHUB_ACTIONS", true)]
    [TestCase(BuildAgent.CircleCI, "APPVEYOR", true)]
    [TestCase(BuildAgent.CircleCI, "TRAVIS", true)]
    [TestCase(BuildAgent.CircleCI, "CODEBUILD_BUILD_ID", false)]
    [TestCase(BuildAgent.CircleCI, "BUILD_ID", false)]
    [TestCase(BuildAgent.CircleCI, "TEAMCITY_VERSION", false)]
    [TestCase(BuildAgent.CircleCI, "JB_SPACE_API_URL", false)]
    [TestCase(BuildAgent.AWSCodeBuild, "CI", true)]
    [TestCase(BuildAgent.AWSCodeBuild, "TF_BUILD", true)]
    [TestCase(BuildAgent.AWSCodeBuild, "GITHUB_ACTIONS", true)]
    [TestCase(BuildAgent.AWSCodeBuild, "APPVEYOR", true)]
    [TestCase(BuildAgent.AWSCodeBuild, "TRAVIS", true)]
    [TestCase(BuildAgent.AWSCodeBuild, "CIRCLECI", true)]
    [TestCase(BuildAgent.AWSCodeBuild, "BUILD_ID", false)]
    [TestCase(BuildAgent.AWSCodeBuild, "TEAMCITY_VERSION", false)]
    [TestCase(BuildAgent.AWSCodeBuild, "JB_SPACE_API_URL", false)]
    [TestCase(BuildAgent.JenkinsOrGoogleCloud, "CI", true)]
    [TestCase(BuildAgent.JenkinsOrGoogleCloud, "TF_BUILD", true)]
    [TestCase(BuildAgent.JenkinsOrGoogleCloud, "GITHUB_ACTIONS", true)]
    [TestCase(BuildAgent.JenkinsOrGoogleCloud, "APPVEYOR", true)]
    [TestCase(BuildAgent.JenkinsOrGoogleCloud, "TRAVIS", true)]
    [TestCase(BuildAgent.JenkinsOrGoogleCloud, "CIRCLECI", true)]
    [TestCase(BuildAgent.JenkinsOrGoogleCloud, "CODEBUILD_BUILD_ID", false)]
    [TestCase(BuildAgent.JenkinsOrGoogleCloud, "TEAMCITY_VERSION", false)]
    [TestCase(BuildAgent.JenkinsOrGoogleCloud, "JB_SPACE_API_URL", false)]
    [TestCase(BuildAgent.TeamCity, "CI", true)]
    [TestCase(BuildAgent.TeamCity, "TF_BUILD", true)]
    [TestCase(BuildAgent.TeamCity, "GITHUB_ACTIONS", true)]
    [TestCase(BuildAgent.TeamCity, "APPVEYOR", true)]
    [TestCase(BuildAgent.TeamCity, "TRAVIS", true)]
    [TestCase(BuildAgent.TeamCity, "CIRCLECI", true)]
    [TestCase(BuildAgent.TeamCity, "CODEBUILD_BUILD_ID", false)]
    [TestCase(BuildAgent.TeamCity, "BUILD_ID", false)]
    [TestCase(BuildAgent.TeamCity, "JB_SPACE_API_URL", false)]
    [TestCase(BuildAgent.JetBrainsSpace, "CI", true)]
    [TestCase(BuildAgent.JetBrainsSpace, "TF_BUILD", true)]
    [TestCase(BuildAgent.JetBrainsSpace, "GITHUB_ACTIONS", true)]
    [TestCase(BuildAgent.JetBrainsSpace, "APPVEYOR", true)]
    [TestCase(BuildAgent.JetBrainsSpace, "TRAVIS", true)]
    [TestCase(BuildAgent.JetBrainsSpace, "CIRCLECI", true)]
    [TestCase(BuildAgent.JetBrainsSpace, "CODEBUILD_BUILD_ID", false)]
    [TestCase(BuildAgent.JetBrainsSpace, "BUILD_ID", false)]
    [TestCase(BuildAgent.JetBrainsSpace, "TEAMCITY_VERSION", false)]
    public void on_conditionally_agent_environment_variable_level_1_true(BuildAgent agent, string variable, bool expectTrue) => agent.Run(analyzer => analyzer
        .ForInlineCsproj(@$"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <RestorePackagesWithLockFile>true</RestorePackagesWithLockFile>
              </PropertyGroup>

              <PropertyGroup Condition=""'$({variable})' {(expectTrue ? "== 'true'" : "!= ''")}"">
                <RestoreLockedMode>true</RestoreLockedMode>
              </PropertyGroup>
            </Project>")
        .HasIssue(
            Issue.WRN("Proj0044", "Define the <RestoreLockedMode> node with value 'true' when <ContinuousIntegrationBuild> is enabled")));

    [TestCase(BuildAgent.AnyCI, "TF_BUILD", true)]
    [TestCase(BuildAgent.AnyCI, "GITHUB_ACTIONS", true)]
    [TestCase(BuildAgent.AnyCI, "APPVEYOR", true)]
    [TestCase(BuildAgent.AnyCI, "TRAVIS", true)]
    [TestCase(BuildAgent.AnyCI, "CIRCLECI", true)]
    [TestCase(BuildAgent.AnyCI, "CODEBUILD_BUILD_ID", false)]
    [TestCase(BuildAgent.AnyCI, "BUILD_ID", false)]
    [TestCase(BuildAgent.AnyCI, "TEAMCITY_VERSION", false)]
    [TestCase(BuildAgent.AnyCI, "JB_SPACE_API_URL", false)]
    [TestCase(BuildAgent.AzurePipelines, "CI", true)]
    [TestCase(BuildAgent.AzurePipelines, "GITHUB_ACTIONS", true)]
    [TestCase(BuildAgent.AzurePipelines, "APPVEYOR", true)]
    [TestCase(BuildAgent.AzurePipelines, "TRAVIS", true)]
    [TestCase(BuildAgent.AzurePipelines, "CIRCLECI", true)]
    [TestCase(BuildAgent.AzurePipelines, "CODEBUILD_BUILD_ID", false)]
    [TestCase(BuildAgent.AzurePipelines, "BUILD_ID", false)]
    [TestCase(BuildAgent.AzurePipelines, "TEAMCITY_VERSION", false)]
    [TestCase(BuildAgent.AzurePipelines, "JB_SPACE_API_URL", false)]
    [TestCase(BuildAgent.GitHubActions, "CI", true)]
    [TestCase(BuildAgent.GitHubActions, "TF_BUILD", true)]
    [TestCase(BuildAgent.GitHubActions, "APPVEYOR", true)]
    [TestCase(BuildAgent.GitHubActions, "TRAVIS", true)]
    [TestCase(BuildAgent.GitHubActions, "CIRCLECI", true)]
    [TestCase(BuildAgent.GitHubActions, "CODEBUILD_BUILD_ID", false)]
    [TestCase(BuildAgent.GitHubActions, "BUILD_ID", false)]
    [TestCase(BuildAgent.GitHubActions, "TEAMCITY_VERSION", false)]
    [TestCase(BuildAgent.GitHubActions, "JB_SPACE_API_URL", false)]
    [TestCase(BuildAgent.AppVeyor, "CI", true)]
    [TestCase(BuildAgent.AppVeyor, "TF_BUILD", true)]
    [TestCase(BuildAgent.AppVeyor, "GITHUB_ACTIONS", true)]
    [TestCase(BuildAgent.AppVeyor, "TRAVIS", true)]
    [TestCase(BuildAgent.AppVeyor, "CIRCLECI", true)]
    [TestCase(BuildAgent.AppVeyor, "CODEBUILD_BUILD_ID", false)]
    [TestCase(BuildAgent.AppVeyor, "BUILD_ID", false)]
    [TestCase(BuildAgent.AppVeyor, "TEAMCITY_VERSION", false)]
    [TestCase(BuildAgent.AppVeyor, "JB_SPACE_API_URL", false)]
    [TestCase(BuildAgent.TravisCI, "CI", true)]
    [TestCase(BuildAgent.TravisCI, "TF_BUILD", true)]
    [TestCase(BuildAgent.TravisCI, "GITHUB_ACTIONS", true)]
    [TestCase(BuildAgent.TravisCI, "APPVEYOR", true)]
    [TestCase(BuildAgent.TravisCI, "CIRCLECI", true)]
    [TestCase(BuildAgent.TravisCI, "CODEBUILD_BUILD_ID", false)]
    [TestCase(BuildAgent.TravisCI, "BUILD_ID", false)]
    [TestCase(BuildAgent.TravisCI, "TEAMCITY_VERSION", false)]
    [TestCase(BuildAgent.TravisCI, "JB_SPACE_API_URL", false)]
    [TestCase(BuildAgent.CircleCI, "CI", true)]
    [TestCase(BuildAgent.CircleCI, "TF_BUILD", true)]
    [TestCase(BuildAgent.CircleCI, "GITHUB_ACTIONS", true)]
    [TestCase(BuildAgent.CircleCI, "APPVEYOR", true)]
    [TestCase(BuildAgent.CircleCI, "TRAVIS", true)]
    [TestCase(BuildAgent.CircleCI, "CODEBUILD_BUILD_ID", false)]
    [TestCase(BuildAgent.CircleCI, "BUILD_ID", false)]
    [TestCase(BuildAgent.CircleCI, "TEAMCITY_VERSION", false)]
    [TestCase(BuildAgent.CircleCI, "JB_SPACE_API_URL", false)]
    [TestCase(BuildAgent.AWSCodeBuild, "CI", true)]
    [TestCase(BuildAgent.AWSCodeBuild, "TF_BUILD", true)]
    [TestCase(BuildAgent.AWSCodeBuild, "GITHUB_ACTIONS", true)]
    [TestCase(BuildAgent.AWSCodeBuild, "APPVEYOR", true)]
    [TestCase(BuildAgent.AWSCodeBuild, "TRAVIS", true)]
    [TestCase(BuildAgent.AWSCodeBuild, "CIRCLECI", true)]
    [TestCase(BuildAgent.AWSCodeBuild, "BUILD_ID", false)]
    [TestCase(BuildAgent.AWSCodeBuild, "TEAMCITY_VERSION", false)]
    [TestCase(BuildAgent.AWSCodeBuild, "JB_SPACE_API_URL", false)]
    [TestCase(BuildAgent.JenkinsOrGoogleCloud, "CI", true)]
    [TestCase(BuildAgent.JenkinsOrGoogleCloud, "TF_BUILD", true)]
    [TestCase(BuildAgent.JenkinsOrGoogleCloud, "GITHUB_ACTIONS", true)]
    [TestCase(BuildAgent.JenkinsOrGoogleCloud, "APPVEYOR", true)]
    [TestCase(BuildAgent.JenkinsOrGoogleCloud, "TRAVIS", true)]
    [TestCase(BuildAgent.JenkinsOrGoogleCloud, "CIRCLECI", true)]
    [TestCase(BuildAgent.JenkinsOrGoogleCloud, "CODEBUILD_BUILD_ID", false)]
    [TestCase(BuildAgent.JenkinsOrGoogleCloud, "TEAMCITY_VERSION", false)]
    [TestCase(BuildAgent.JenkinsOrGoogleCloud, "JB_SPACE_API_URL", false)]
    [TestCase(BuildAgent.TeamCity, "CI", true)]
    [TestCase(BuildAgent.TeamCity, "TF_BUILD", true)]
    [TestCase(BuildAgent.TeamCity, "GITHUB_ACTIONS", true)]
    [TestCase(BuildAgent.TeamCity, "APPVEYOR", true)]
    [TestCase(BuildAgent.TeamCity, "TRAVIS", true)]
    [TestCase(BuildAgent.TeamCity, "CIRCLECI", true)]
    [TestCase(BuildAgent.TeamCity, "CODEBUILD_BUILD_ID", false)]
    [TestCase(BuildAgent.TeamCity, "BUILD_ID", false)]
    [TestCase(BuildAgent.TeamCity, "JB_SPACE_API_URL", false)]
    [TestCase(BuildAgent.JetBrainsSpace, "CI", true)]
    [TestCase(BuildAgent.JetBrainsSpace, "TF_BUILD", true)]
    [TestCase(BuildAgent.JetBrainsSpace, "GITHUB_ACTIONS", true)]
    [TestCase(BuildAgent.JetBrainsSpace, "APPVEYOR", true)]
    [TestCase(BuildAgent.JetBrainsSpace, "TRAVIS", true)]
    [TestCase(BuildAgent.JetBrainsSpace, "CIRCLECI", true)]
    [TestCase(BuildAgent.JetBrainsSpace, "CODEBUILD_BUILD_ID", false)]
    [TestCase(BuildAgent.JetBrainsSpace, "BUILD_ID", false)]
    [TestCase(BuildAgent.JetBrainsSpace, "TEAMCITY_VERSION", false)]
    public void on_conditionally_agent_environment_variable_level_2_true(BuildAgent agent, string variable, bool expectTrue) => agent.Run(analyzer => analyzer
        .ForInlineCsproj(@$"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <RestorePackagesWithLockFile>true</RestorePackagesWithLockFile>
                <RestoreLockedMode Condition=""'$({variable})' {(expectTrue ? "== 'true'" : "!= ''")}"">true</RestoreLockedMode>
              </PropertyGroup>
            </Project>")
        .HasIssue(
            Issue.WRN("Proj0044", "Define the <RestoreLockedMode> node with value 'true' when <ContinuousIntegrationBuild> is enabled")));
}

[NonParallelizable]
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

    [TestCase(BuildAgent.AnyCI, "CI", true)]
    [TestCase(BuildAgent.AzurePipelines, "TF_BUILD", true)]
    [TestCase(BuildAgent.GitHubActions, "GITHUB_ACTIONS", true)]
    [TestCase(BuildAgent.AppVeyor, "APPVEYOR", true)]
    [TestCase(BuildAgent.TravisCI, "TRAVIS", true)]
    [TestCase(BuildAgent.CircleCI, "CIRCLECI", true)]
    [TestCase(BuildAgent.AWSCodeBuild, "CODEBUILD_BUILD_ID", false)]
    [TestCase(BuildAgent.JenkinsOrGoogleCloud, "BUILD_ID", false)]
    [TestCase(BuildAgent.TeamCity, "TEAMCITY_VERSION", false)]
    [TestCase(BuildAgent.JetBrainsSpace, "JB_SPACE_API_URL", false)]
    [TestCase(BuildAgent.Local, "CI", true)]
    [TestCase(BuildAgent.Local, "TF_BUILD", true)]
    [TestCase(BuildAgent.Local, "GITHUB_ACTIONS", true)]
    [TestCase(BuildAgent.Local, "APPVEYOR", true)]
    [TestCase(BuildAgent.Local, "TRAVIS", true)]
    [TestCase(BuildAgent.Local, "CIRCLECI", true)]
    [TestCase(BuildAgent.Local, "CODEBUILD_BUILD_ID", false)]
    [TestCase(BuildAgent.Local, "BUILD_ID", false)]
    [TestCase(BuildAgent.Local, "TEAMCITY_VERSION", false)]
    [TestCase(BuildAgent.Local, "JB_SPACE_API_URL", false)]
    public void on_conditionally_agent_environment_variable_level_1_true(BuildAgent agent, string variable, bool expectTrue) => agent.Run(analyzer => analyzer
        .ForInlineCsproj(@$"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <RestorePackagesWithLockFile>true</RestorePackagesWithLockFile>
              </PropertyGroup>

              <PropertyGroup Condition=""'$({variable})' {(expectTrue ? "== 'true'" : "!= ''")}"">
                <RestoreLockedMode>true</RestoreLockedMode>
              </PropertyGroup>
            </Project>")
        .HasNoIssues());

    [TestCase(BuildAgent.AnyCI, "CI", true)]
    [TestCase(BuildAgent.AzurePipelines, "TF_BUILD", true)]
    [TestCase(BuildAgent.GitHubActions, "GITHUB_ACTIONS", true)]
    [TestCase(BuildAgent.AppVeyor, "APPVEYOR", true)]
    [TestCase(BuildAgent.TravisCI, "TRAVIS", true)]
    [TestCase(BuildAgent.CircleCI, "CIRCLECI", true)]
    [TestCase(BuildAgent.AWSCodeBuild, "CODEBUILD_BUILD_ID", false)]
    [TestCase(BuildAgent.JenkinsOrGoogleCloud, "BUILD_ID", false)]
    [TestCase(BuildAgent.TeamCity, "TEAMCITY_VERSION", false)]
    [TestCase(BuildAgent.JetBrainsSpace, "JB_SPACE_API_URL", false)]
    [TestCase(BuildAgent.Local, "CI", true)]
    [TestCase(BuildAgent.Local, "TF_BUILD", true)]
    [TestCase(BuildAgent.Local, "GITHUB_ACTIONS", true)]
    [TestCase(BuildAgent.Local, "APPVEYOR", true)]
    [TestCase(BuildAgent.Local, "TRAVIS", true)]
    [TestCase(BuildAgent.Local, "CIRCLECI", true)]
    [TestCase(BuildAgent.Local, "CODEBUILD_BUILD_ID", false)]
    [TestCase(BuildAgent.Local, "BUILD_ID", false)]
    [TestCase(BuildAgent.Local, "TEAMCITY_VERSION", false)]
    [TestCase(BuildAgent.Local, "JB_SPACE_API_URL", false)]
    public void on_conditionally_agent_environment_variable_level_2_true(BuildAgent agent, string variable, bool expectTrue) => agent.Run(analyzer => analyzer
        .ForInlineCsproj(@$"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <RestorePackagesWithLockFile>true</RestorePackagesWithLockFile>
                <RestoreLockedMode Condition=""'$({variable})' {(expectTrue ? "== 'true'" : "!= ''")}"">true</RestoreLockedMode>
              </PropertyGroup>
            </Project>")
        .HasNoIssues());
}

file static class Helpers
{
    public static void Run(this BuildAgent agent, Action<EnableRestoreLockedMode> act)
        => agent.Run<EnableRestoreLockedMode>(act);
}
