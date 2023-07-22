namespace Rules.RESX.Sort_data_alphabetically;

public class Reports
{
    [Test]
    public void unsorted_data()
        => new Resx.SortDataAlphabetically()
        .ForProject("ResxUnsorted.cs")
        .HasIssue(
            new Issue("Proj2002", "Resource 'B' is not ordered alphabetically and should appear before 'C'.").WithSpan(20, 3, 22, 9));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    public void sorted_data(string project)
         => new Resx.SortDataAlphabetically()
        .ForProject(project)
        .HasNoIssues();
}

public class Ignores
{
    [Test]
    public void invalid_resources()
         => new Resx.SortDataAlphabetically()
        .ForProject("ResxNoXml.cs")
        .HasNoIssues();
}
