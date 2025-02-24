namespace Rules.RESX.Define_data_in_resource;

public class Reports
{
    [Test]
    public void unsorted_data()
        => new Resx.DefineData()
        .ForProject("ResxNoData.cs")
        .HasIssue(Issue.WRN("Proj2001", "Resource does not contain any data"));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    public void sorted_data(string project)
         => new Resx.DefineData()
        .ForProject(project)
        .HasNoIssues();
}

public class Ignores
{
    [Ignore("Does not compile.")]
    [Test]
    public void invalid_resources()
         => new Resx.DefineData()
        .ForProject("ResxNoXml.cs")
        .HasNoIssues();
}
