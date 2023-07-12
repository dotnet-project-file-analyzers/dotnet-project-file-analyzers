namespace Rules.RESX.Add_invariant_resources;

public class Reports
{
    [Test]
    public void missing_invariant_resource()
        => new Resx.AddInvariantResources()
        .ForProject("ResxNoInvariant.cs")
        .HasIssue(
            new Issue("Proj2003", "Add invariant resource."));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantVB.vb")]
    public void sorted_data(string project)
         => new Resx.AddInvariantResources()
        .ForProject(project)
        .HasNoIssues();
}

public class Ignores
{
    [Test]
    public void invalid_resources()
         => new Resx.AddInvariantResources()
        .ForProject("ResxNoXml.cs")
        .HasNoIssues();
}
