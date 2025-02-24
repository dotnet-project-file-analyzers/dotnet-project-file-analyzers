namespace Rules.MS_Build.Define_conditions_on_level_1;

public class Reports
{
    [Test]
    public void conditions_on_other_levels()
        => new DefineConditionsOnLevel1()
        .ForProject("Conditional.cs")
        .HasIssue(Issue.WRN("Proj0028", "Move the condition to the parent <PropertyGroup>")
        .WithSpan(11, 04, 11, 76));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project)
         => new DefineConditionsOnLevel1()
        .ForProject(project)
        .HasNoIssues();
}
