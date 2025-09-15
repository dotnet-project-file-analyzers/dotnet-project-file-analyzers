namespace Rules.SLNX.Indent_SLNX;

public class Reports
{
    [Test]
    public void Faulty_indented() => new Slnx.IndentXml()
        .ForSdkProject("SolutionFile")
        .HasIssue(Issue.WRN("Proj1700", "The element <Project> has not been properly indented")
            .WithSpan(04, 00, 04, 42)
            .WithPath("faulty-indented.slnx"));

    [Test]
    public void Faulty_indented_inline() => new Slnx.IndentXml()
        .ForInlineSlnx("""
<Solution>
  <Folder Name="/Solution Items/">
    <File Path=".net.csproj" />
  </Folder>
<Project Path="src/SolutionFile.csproj" />
</Solution>
""")
        .HasIssue(Issue.WRN("Proj1700", "The element <Project> has not been properly indented")
            .WithSpan(04, 00, 04, 42));
}
