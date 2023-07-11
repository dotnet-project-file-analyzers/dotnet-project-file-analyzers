namespace Rules.RESX.Sort_data_alphabetically;

public class Reports
{
    [Test]
    public void unsorted_data()
        => new Resx.SortDataAlphabetically()
        .ForProject("ResxUnsorted.cs")
        .HasIssue(
            new Issue("Proj2002", "Resource values should be ordered alphabetically by their names.").WithSpan(17, 3, 19, 9));
}


public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantVB.vb")]
    public void sorted_data(string project)
         => new Resx.SortDataAlphabetically()
        .ForProject(project)
        .HasNoIssues();
}
