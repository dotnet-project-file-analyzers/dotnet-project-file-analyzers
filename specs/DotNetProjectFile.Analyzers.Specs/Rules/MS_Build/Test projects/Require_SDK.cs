namespace Rules.MS_Build.Test_projects.Require_SDK;

public class Reports
{
    [Test]
    public void publishable_test_project()
        => new TestProjectsRequireSdk()
        .ForProject("TestProjectWithoutSdk.cs")
        .HasIssue(Issue.WRN("Proj0452", @"Include <PackageReference Include=""Microsoft.NET.Test.Sdk"" PrivateAssets =""all"" />")
        .WithSpan(00, 00, 00, 32));

    [Test]
    public void SDK_without_test_project()
        => new TestProjectsRequireSdk()
        .ForProject("TestSdkOnly.cs")
        .HasIssue(Issue.WRN("Proj0453", "Set <IsTestProject> to true")
        .WithSpan(00, 00, 00, 32));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    [TestCase("TestProject.cs")]
    [TestCase("TUnitTestProject.cs")]
    public void non_test_project(string project)
         => new TestProjectsRequireSdk()
        .ForProject(project)
        .HasNoIssues();
}
