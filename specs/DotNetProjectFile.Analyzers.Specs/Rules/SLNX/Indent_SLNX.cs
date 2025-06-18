namespace Rules.SLNX.Indent_SLNX;

public class Reports
{
    [Test]
    public void Faulty_indented() => new Slnx.IndentXml()
        .ForSDkProject("SolutionFile")
        .HasIssue(Issue.WRN("Proj1700", "The element <Project> has not been properly indented")
            .WithSpan(04, 00, 04, 42)
            .WithPath("faulty-indented.slnx"));
}
