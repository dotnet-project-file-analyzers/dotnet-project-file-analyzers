namespace Rules.MS_Build.Reassign_properties_with_different_value;

public class Reports
{
    [Test]
    public void on_reassignments_with_same_value()
       => new ReassignPropertiesWithDifferentValue()
       .ForProject("ReassignProperties.cs")
       .HasIssues(
            Issue.WRN("Proj0012", "Property <OutputType> has been previously defined with the same value." /*.*/).WithSpan(05, 04, 05, 36),
            Issue.WRN("Proj0012", "Property <Nullable> has been previously defined with the same value." /*...*/).WithSpan(06, 04, 06, 31),
            Issue.WRN("Proj0012", "Property <OutputType> has been previously defined with the same value." /*.*/).WithSpan(07, 04, 07, 36));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    public void Projects_without_issues(string project)
         => new ReassignPropertiesWithDifferentValue()
        .ForProject(project)
        .HasNoIssues();
}
