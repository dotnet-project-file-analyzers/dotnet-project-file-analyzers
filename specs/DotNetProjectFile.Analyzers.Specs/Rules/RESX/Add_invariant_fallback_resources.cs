namespace Rules.RESX.Add_invariant_fallback_resources;

public class Reports
{
    [Test]
    public void missing_invariant_resource()
        => new Resx.AddInvariantFallbackResources()
        .ForProject("ResxNoInvariant.cs")
        .HasIssue(
            new Issue("Proj2003", "Add invariant fallback resource."));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    public void sorted_data(string project)
         => new Resx.AddInvariantFallbackResources()
        .ForProject(project)
        .HasNoIssues();
}

public class Ignores
{
    [Test]
    public void invalid_resources()
         => new Resx.AddInvariantFallbackResources()
        .ForProject("ResxNoXml.cs")
        .HasNoIssues();
}
