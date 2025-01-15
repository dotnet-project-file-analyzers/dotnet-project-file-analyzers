namespace Rules.MS_Build.Test_projects.Require_SDK;

public class Reports
{
    [Test]
    public void publishable_test_project()
        => new TestProjectsRequireSdk()
        .ForProject("TestProjectWithoutSdk.cs")
        .HasIssue(new Issue("Proj0452", @"Include <PackageReference Include=""Microsoft.NET.Test.Sdk"" PrivateAssets =""all"" />.")
        .WithSpan(00, 00, 11, 10));

    [Test]
    public void SDK_without_test_project()
        => new TestProjectsRequireSdk()
        .ForProject("TestSdkOnly.cs")
        .HasIssue(new Issue("Proj0453", "Set <IsTestProject> to true.")
        .WithSpan(00, 00, 14, 10));
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
