namespace Rules.MS_Build.Reassign_properties_with_different_value;

public class Reports
{
    [Test]
    public void on_reassignments_with_same_value()
       => new ReassignPropertiesWithDifferentValue()
       .ForProject("ReassignProperties.cs")
       .HasIssues(
            new Issue("Proj0012", "Property <OutputType> has previously be defined with the same value.").WithSpan(/*.*/6, 5, 6, 36),
            new Issue("Proj0012", "Property <Nullable> has previously be defined with the same value.").WithSpan(/*...*/6, 5, 6, 31),
            new Issue("Proj0012", "Property <OutputType> has previously be defined with the same value.").WithSpan(/*.*/7, 5, 7, 36));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantVB.vb")]
    public void Projects_without_issues(string project)
         => new ReassignPropertiesWithDifferentValue()
        .ForProject(project)
        .HasNoIssues();
}
