namespace Rules.MS_Build.Define_single_target_framework;

public class Reports
{
    [Test]
    public void defined_in_target_frameworks()
        => new DefineSingleTargetFramework()
        .ForProject("TargetFrameworksSingle.cs")
        .HasIssues(
            Issue.WRN("Proj0009", "Use the <TargetFramework> node instead").WithSpan(3, 4, 3, 47));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    [TestCase("TargetFrameworksMultiple.cs")]
    [TestCase("TargetFrameworksSingleWithBase.cs")]
    public void Projects_with_analyzers(string project)
         => new DefineSingleTargetFramework()
        .ForProject(project)
        .HasNoIssues();
}
