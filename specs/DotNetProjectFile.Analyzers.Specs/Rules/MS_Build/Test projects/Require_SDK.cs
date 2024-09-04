namespace Rules.MS_Build.Test_projects.Require_SDK;

public class Reports
{
    [Test]
    public void publishable_test_project()
        => new TestProjectsRequireSdk()
        .ForProject("TestProjectWithoutSdk.cs")
        .HasIssue(new Issue("Proj0452", @"Include <PackageReference Include=""Microsoft.NET.Test.Sdk"" PrivateAssets =""all"" />.")
        .WithSpan(00, 00, 11, 10));
}

public class Guards
{
    [Test]
    public void non_publishable_test_project()
        => new TestProjectsRequireSdk()
        .ForProject("TestProject.cs")
        .HasNoIssues();

    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void non_test_project(string project)
         => new TestProjectsRequireSdk()
        .ForProject(project)
        .HasNoIssues();
}
