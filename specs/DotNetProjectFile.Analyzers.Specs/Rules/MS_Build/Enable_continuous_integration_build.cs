using DotNetProjectFile.BuildAgents;

namespace Rules.MS_Build.Enable_continuous_integration_build;

[NonParallelizable]
public class Reports
{
    [Test]
    public void on_missing() => BuildAgent.Local.Run(analyzer => analyzer
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
              </PropertyGroup>
            </Project>")
        .HasIssue(
            Issue.WRN("Proj0042", "Define the <ContinuousIntegrationBuild> node with value 'true' when running in CI pipeline").WithSpan(0, 0, 0, 32)));

    [Test]
    public void on_false() => BuildAgent.Local.Run(analyzer => analyzer
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <ContinuousIntegrationBuild>false</ContinuousIntegrationBuild>
              </PropertyGroup>
            </Project>")
        .HasIssue(
            Issue.WRN("Proj0042", "Define the <ContinuousIntegrationBuild> node with value 'true' when running in CI pipeline").WithSpan(0, 0, 0, 32)));

    [Test]
    public void on_unconditially_true() => BuildAgent.Local.Run(analyzer => analyzer
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <ContinuousIntegrationBuild>true</ContinuousIntegrationBuild>
              </PropertyGroup>
            </Project>")
        .HasIssue(
            Issue.WRN("Proj0042", "Define the <ContinuousIntegrationBuild> node with value 'true' when running in CI pipeline").WithSpan(0, 0, 0, 32)));

    [TestCase(BuildAgent.AzurePipelines)]
    [TestCase(BuildAgent.GitHubActions)]
    [TestCase(BuildAgent.AppVeyor)]
    [TestCase(BuildAgent.TravisCI)]
    [TestCase(BuildAgent.CircleCI)]
    [TestCase(BuildAgent.AWSCodeBuild)]
    [TestCase(BuildAgent.JenkinsOrGoogleCloud)]
    [TestCase(BuildAgent.TeamCity)]
    [TestCase(BuildAgent.JetBrainsSpace)]
    public void on_conditionally_true_any_ci(BuildAgent agent) => agent.Run(analyzer => analyzer
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <ContinuousIntegrationBuild Condition=""'$(CI)' == 'true'"">true</ContinuousIntegrationBuild>
              </PropertyGroup>
            </Project>")
        .HasIssue(
            Issue.WRN("Proj0042", "Define the <ContinuousIntegrationBuild> node with value 'true' when running in CI pipeline").WithSpan(0, 0, 0, 32)));

    [TestCase(BuildAgent.AnyCI)]
    [TestCase(BuildAgent.GitHubActions)]
    [TestCase(BuildAgent.AppVeyor)]
    [TestCase(BuildAgent.TravisCI)]
    [TestCase(BuildAgent.CircleCI)]
    [TestCase(BuildAgent.AWSCodeBuild)]
    [TestCase(BuildAgent.JenkinsOrGoogleCloud)]
    [TestCase(BuildAgent.TeamCity)]
    [TestCase(BuildAgent.JetBrainsSpace)]
    public void on_conditionally_true_azure(BuildAgent agent) => agent.Run(analyzer => analyzer
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <ContinuousIntegrationBuild Condition=""'$(TF_BUILD)' == 'true'"">true</ContinuousIntegrationBuild>
              </PropertyGroup>
            </Project>")
        .HasIssue(
            Issue.WRN("Proj0042", "Define the <ContinuousIntegrationBuild> node with value 'true' when running in CI pipeline").WithSpan(0, 0, 0, 32)));

    [TestCase(BuildAgent.AnyCI)]
    [TestCase(BuildAgent.AzurePipelines)]
    [TestCase(BuildAgent.AppVeyor)]
    [TestCase(BuildAgent.TravisCI)]
    [TestCase(BuildAgent.CircleCI)]
    [TestCase(BuildAgent.AWSCodeBuild)]
    [TestCase(BuildAgent.JenkinsOrGoogleCloud)]
    [TestCase(BuildAgent.TeamCity)]
    [TestCase(BuildAgent.JetBrainsSpace)]
    public void on_conditionally_true_github(BuildAgent agent) => agent.Run(analyzer => analyzer
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <ContinuousIntegrationBuild Condition=""'$(GITHUB_ACTIONS)' == 'true'"">true</ContinuousIntegrationBuild>
              </PropertyGroup>
            </Project>")
        .HasIssue(
            Issue.WRN("Proj0042", "Define the <ContinuousIntegrationBuild> node with value 'true' when running in CI pipeline").WithSpan(0, 0, 0, 32)));

    [TestCase(BuildAgent.AnyCI)]
    [TestCase(BuildAgent.AzurePipelines)]
    [TestCase(BuildAgent.GitHubActions)]
    [TestCase(BuildAgent.TravisCI)]
    [TestCase(BuildAgent.CircleCI)]
    [TestCase(BuildAgent.AWSCodeBuild)]
    [TestCase(BuildAgent.JenkinsOrGoogleCloud)]
    [TestCase(BuildAgent.TeamCity)]
    [TestCase(BuildAgent.JetBrainsSpace)]
    public void on_conditionally_true_appveyor(BuildAgent agent) => agent.Run(analyzer => analyzer
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <ContinuousIntegrationBuild Condition=""'$(APPVEYOR)' == 'true'"">true</ContinuousIntegrationBuild>
              </PropertyGroup>
            </Project>")
        .HasIssue(
            Issue.WRN("Proj0042", "Define the <ContinuousIntegrationBuild> node with value 'true' when running in CI pipeline").WithSpan(0, 0, 0, 32)));

    [TestCase(BuildAgent.AnyCI)]
    [TestCase(BuildAgent.AzurePipelines)]
    [TestCase(BuildAgent.GitHubActions)]
    [TestCase(BuildAgent.AppVeyor)]
    [TestCase(BuildAgent.CircleCI)]
    [TestCase(BuildAgent.AWSCodeBuild)]
    [TestCase(BuildAgent.JenkinsOrGoogleCloud)]
    [TestCase(BuildAgent.TeamCity)]
    [TestCase(BuildAgent.JetBrainsSpace)]
    public void on_conditionally_true_travis(BuildAgent agent) => agent.Run(analyzer => analyzer
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <ContinuousIntegrationBuild Condition=""'$(TRAVIS)' == 'true'"">true</ContinuousIntegrationBuild>
              </PropertyGroup>
            </Project>")
        .HasIssue(
            Issue.WRN("Proj0042", "Define the <ContinuousIntegrationBuild> node with value 'true' when running in CI pipeline").WithSpan(0, 0, 0, 32)));

    [TestCase(BuildAgent.AnyCI)]
    [TestCase(BuildAgent.AzurePipelines)]
    [TestCase(BuildAgent.GitHubActions)]
    [TestCase(BuildAgent.AppVeyor)]
    [TestCase(BuildAgent.TravisCI)]
    [TestCase(BuildAgent.AWSCodeBuild)]
    [TestCase(BuildAgent.JenkinsOrGoogleCloud)]
    [TestCase(BuildAgent.TeamCity)]
    [TestCase(BuildAgent.JetBrainsSpace)]
    public void on_conditionally_true_circleci(BuildAgent agent) => agent.Run(analyzer => analyzer
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <ContinuousIntegrationBuild Condition=""'$(CIRCLECI)' == 'true'"">true</ContinuousIntegrationBuild>
              </PropertyGroup>
            </Project>")
        .HasIssue(
            Issue.WRN("Proj0042", "Define the <ContinuousIntegrationBuild> node with value 'true' when running in CI pipeline").WithSpan(0, 0, 0, 32)));

    [TestCase(BuildAgent.AnyCI)]
    [TestCase(BuildAgent.AzurePipelines)]
    [TestCase(BuildAgent.GitHubActions)]
    [TestCase(BuildAgent.AppVeyor)]
    [TestCase(BuildAgent.TravisCI)]
    [TestCase(BuildAgent.CircleCI)]
    [TestCase(BuildAgent.JenkinsOrGoogleCloud)]
    [TestCase(BuildAgent.TeamCity)]
    [TestCase(BuildAgent.JetBrainsSpace)]
    public void on_conditionally_true_aws(BuildAgent agent) => agent.Run(analyzer => analyzer
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <ContinuousIntegrationBuild Condition=""'$(CODEBUILD_BUILD_ID)' != ''"">true</ContinuousIntegrationBuild>
              </PropertyGroup>
            </Project>")
        .HasIssue(
            Issue.WRN("Proj0042", "Define the <ContinuousIntegrationBuild> node with value 'true' when running in CI pipeline").WithSpan(0, 0, 0, 32)));

    [TestCase(BuildAgent.AnyCI)]
    [TestCase(BuildAgent.AzurePipelines)]
    [TestCase(BuildAgent.GitHubActions)]
    [TestCase(BuildAgent.AppVeyor)]
    [TestCase(BuildAgent.TravisCI)]
    [TestCase(BuildAgent.CircleCI)]
    [TestCase(BuildAgent.AWSCodeBuild)]
    [TestCase(BuildAgent.TeamCity)]
    [TestCase(BuildAgent.JetBrainsSpace)]
    public void on_conditionally_true_jenkins(BuildAgent agent) => agent.Run(analyzer => analyzer
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <ContinuousIntegrationBuild Condition=""'$(BUILD_ID)' != ''"">true</ContinuousIntegrationBuild>
              </PropertyGroup>
            </Project>")
        .HasIssue(
            Issue.WRN("Proj0042", "Define the <ContinuousIntegrationBuild> node with value 'true' when running in CI pipeline").WithSpan(0, 0, 0, 32)));

    [TestCase(BuildAgent.AnyCI)]
    [TestCase(BuildAgent.AzurePipelines)]
    [TestCase(BuildAgent.GitHubActions)]
    [TestCase(BuildAgent.AppVeyor)]
    [TestCase(BuildAgent.TravisCI)]
    [TestCase(BuildAgent.CircleCI)]
    [TestCase(BuildAgent.AWSCodeBuild)]
    [TestCase(BuildAgent.JenkinsOrGoogleCloud)]
    [TestCase(BuildAgent.JetBrainsSpace)]
    public void on_conditionally_true_teamcity(BuildAgent agent) => agent.Run(analyzer => analyzer
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <ContinuousIntegrationBuild Condition=""'$(TEAMCITY_VERSION)' != ''"">true</ContinuousIntegrationBuild>
              </PropertyGroup>
            </Project>")
        .HasIssue(
            Issue.WRN("Proj0042", "Define the <ContinuousIntegrationBuild> node with value 'true' when running in CI pipeline").WithSpan(0, 0, 0, 32)));

    [TestCase(BuildAgent.AnyCI)]
    [TestCase(BuildAgent.AzurePipelines)]
    [TestCase(BuildAgent.GitHubActions)]
    [TestCase(BuildAgent.AppVeyor)]
    [TestCase(BuildAgent.TravisCI)]
    [TestCase(BuildAgent.CircleCI)]
    [TestCase(BuildAgent.AWSCodeBuild)]
    [TestCase(BuildAgent.JenkinsOrGoogleCloud)]
    [TestCase(BuildAgent.TeamCity)]
    public void on_conditionally_true_jetbrains(BuildAgent agent) => agent.Run(analyzer => analyzer
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <ContinuousIntegrationBuild Condition=""'$(JB_SPACE_API_URL)' != ''"">true</ContinuousIntegrationBuild>
              </PropertyGroup>
            </Project>")
        .HasIssue(
            Issue.WRN("Proj0042", "Define the <ContinuousIntegrationBuild> node with value 'true' when running in CI pipeline").WithSpan(0, 0, 0, 32)));
}

[NonParallelizable]
public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void compliant_local(string project) => BuildAgent.Local.Run(analyzer => analyzer
        .ForProject(project)
        .HasNoIssues());

    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void compliant_github(string project) => BuildAgent.GitHubActions.Run(analyzer => analyzer
        .ForProject(project)
        .HasNoIssues());

    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void compliant_azure(string project) => BuildAgent.AzurePipelines.Run(analyzer => analyzer
        .ForProject(project)
        .HasNoIssues());

    [TestCase(BuildAgent.Local)]
    [TestCase(BuildAgent.AnyCI)]
    [TestCase(BuildAgent.AzurePipelines)]
    [TestCase(BuildAgent.GitHubActions)]
    [TestCase(BuildAgent.AppVeyor)]
    [TestCase(BuildAgent.TravisCI)]
    [TestCase(BuildAgent.CircleCI)]
    [TestCase(BuildAgent.AWSCodeBuild)]
    [TestCase(BuildAgent.JenkinsOrGoogleCloud)]
    [TestCase(BuildAgent.TeamCity)]
    [TestCase(BuildAgent.JetBrainsSpace)]
    public void on_reproducible_builds_package(BuildAgent agent) => agent.Run(analyzer => analyzer
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
              </PropertyGroup>

              <ItemGroup>
                <PackageReference Include=""DotNet.ReproducibleBuilds"" Version=""1.2.25"" />
              </ItemGroup>
            </Project>")
        .HasNoIssues());

    [TestCase(BuildAgent.AnyCI)]
    [TestCase(BuildAgent.Local)]
    public void on_conditionally_true_any_ci(BuildAgent agent) => agent.Run(analyzer => analyzer
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <ContinuousIntegrationBuild Condition=""'$(CI)' == 'true'"">true</ContinuousIntegrationBuild>
              </PropertyGroup>
            </Project>")
        .HasNoIssues());

    [TestCase(BuildAgent.AzurePipelines)]
    [TestCase(BuildAgent.Local)]
    public void on_conditionally_true_azure(BuildAgent agent) => agent.Run(analyzer => analyzer
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <ContinuousIntegrationBuild Condition=""'$(TF_BUILD)' == 'true'"">true</ContinuousIntegrationBuild>
              </PropertyGroup>
            </Project>")
        .HasNoIssues());

    [TestCase(BuildAgent.GitHubActions)]
    [TestCase(BuildAgent.Local)]
    public void on_conditionally_true_github(BuildAgent agent) => agent.Run(analyzer => analyzer
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <ContinuousIntegrationBuild Condition=""'$(GITHUB_ACTIONS)' == 'true'"">true</ContinuousIntegrationBuild>
              </PropertyGroup>
            </Project>")
        .HasNoIssues());

    [TestCase(BuildAgent.AppVeyor)]
    [TestCase(BuildAgent.Local)]
    public void on_conditionally_true_appveyor(BuildAgent agent) => agent.Run(analyzer => analyzer
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <ContinuousIntegrationBuild Condition=""'$(APPVEYOR)' == 'true'"">true</ContinuousIntegrationBuild>
              </PropertyGroup>
            </Project>")
        .HasNoIssues());

    [TestCase(BuildAgent.TravisCI)]
    [TestCase(BuildAgent.Local)]
    public void on_conditionally_true_travis(BuildAgent agent) => agent.Run(analyzer => analyzer
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <ContinuousIntegrationBuild Condition=""'$(TRAVIS)' == 'true'"">true</ContinuousIntegrationBuild>
              </PropertyGroup>
            </Project>")
        .HasNoIssues());

    [TestCase(BuildAgent.CircleCI)]
    [TestCase(BuildAgent.Local)]
    public void on_conditionally_true_circleci(BuildAgent agent) => agent.Run(analyzer => analyzer
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <ContinuousIntegrationBuild Condition=""'$(CIRCLECI)' == 'true'"">true</ContinuousIntegrationBuild>
              </PropertyGroup>
            </Project>")
        .HasNoIssues());

    [TestCase(BuildAgent.AWSCodeBuild)]
    [TestCase(BuildAgent.Local)]
    public void on_conditionally_true_aws(BuildAgent agent) => agent.Run(analyzer => analyzer
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <ContinuousIntegrationBuild Condition=""'$(CODEBUILD_BUILD_ID)' != ''"">true</ContinuousIntegrationBuild>
              </PropertyGroup>
            </Project>")
        .HasNoIssues());

    [TestCase(BuildAgent.JenkinsOrGoogleCloud)]
    [TestCase(BuildAgent.Local)]
    public void on_conditionally_true_jenkins(BuildAgent agent) => agent.Run(analyzer => analyzer
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <ContinuousIntegrationBuild Condition=""'$(BUILD_ID)' != ''"">true</ContinuousIntegrationBuild>
              </PropertyGroup>
            </Project>")
        .HasNoIssues());

    [TestCase(BuildAgent.TeamCity)]
    [TestCase(BuildAgent.Local)]
    public void on_conditionally_true_teamcity(BuildAgent agent) => agent.Run(analyzer => analyzer
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <ContinuousIntegrationBuild Condition=""'$(TEAMCITY_VERSION)' != ''"">true</ContinuousIntegrationBuild>
              </PropertyGroup>
            </Project>")
        .HasNoIssues());

    [TestCase(BuildAgent.JetBrainsSpace)]
    [TestCase(BuildAgent.Local)]
    public void on_conditionally_true_jetbrains(BuildAgent agent) => agent.Run(analyzer => analyzer
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <ContinuousIntegrationBuild Condition=""'$(JB_SPACE_API_URL)' != ''"">true</ContinuousIntegrationBuild>
              </PropertyGroup>
            </Project>")
        .HasNoIssues());
}

file static class Helpers
{
    public static void Run(this BuildAgent agent, Action<EnableContinuousIntegrationBuild> act)
    {
        DisableAgents();
        EnableAgent(agent);

        var analyzer = new EnableContinuousIntegrationBuild();

        try
        {
            act(analyzer);
        }
        finally
        {
            DisableAgent(agent);
        }
    }

    private static void DisableAgents()
    {
        foreach (var agent in Enum.GetValues<BuildAgent>())
        {
            DisableAgent(agent);
        }
    }

    private static void DisableAgent(BuildAgent agent)
    {
        var reqs = agent.GetRequirements();
        var vars = reqs.TrueValues.Concat(reqs.NonEmptyValues);
        
        foreach (var name in vars)
        {
            Environment.SetEnvironmentVariable(name, null);
        }
    }

    private static void EnableAgent(BuildAgent agent)
    {
        var reqs = agent.GetRequirements();
        var vars = reqs.TrueValues.Concat(reqs.NonEmptyValues);

        foreach (var name in vars)
        {
            Environment.SetEnvironmentVariable(name, "true");
        }
    }
}
