namespace Rules.MS_Build.Define_single_target_framework;

public class Reports
{
    [Test]
    public void defined_in_target_frameworks()
        => new DefineSingleTargetFramework()
        .ForProject("TargetFrameworksSingle.cs")
        .HasIssues(
            new Issue("Proj0009", "Use the <TargetFramework> node instead.").WithSpan(3, 5, 3, 47));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantVB.vb")]
    [TestCase("TargetFrameworksMultiple.cs")]
    public void Projects_with_analyzers(string project)
         => new DefineSingleTargetFramework()
        .ForProject(project)
        .HasNoIssues();
}
