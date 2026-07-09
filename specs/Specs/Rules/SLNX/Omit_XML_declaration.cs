namespace Rules.SLNX.Omit_XML_declaration;

public class Reports
{
    [Test]
    public void XML_declaration() => new Slnx.OmitXmlDeclarations()
       .ForInlineSlnx("""
        <?xml version="1.0" ?>
        <Solution>
          <Project Path=".net.csproj" />
          <Project Path="src/SolutionFile.csproj" />
        </Solution>
        """)
       .HasIssue(Issue.WRN("Proj1702", "Remove the XML declaration as it is redundant")
           .WithSpan(01, 00, 01, 09));
}
