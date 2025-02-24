namespace Rules.SDK.Avoid_compile_items;

public class Reports
{
    [Test]
    public void SDK_with_compile_items() => new AvoidCompileItemInSdk()
        .ForSDkProject("SdkWithCompileItems")
        .HasIssue(Issue.WRN("Proj0700", "The .net.csproj SDK project should not contain <Compile> items").WithSpan(5, 04, 5, 43));
}

public class Guards
{
    [TestCase("DotnetProjectFilesSdk.sdk")]
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new AvoidCompileItemInSdk()
        .ForProject(project)
        .HasNoIssues();
}
