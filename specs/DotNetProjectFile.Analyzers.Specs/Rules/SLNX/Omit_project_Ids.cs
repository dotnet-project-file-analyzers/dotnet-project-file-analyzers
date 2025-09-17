namespace Specs.Rules.SLNX.Omit_project_Ids;

public class Reports
{
    [Test]
    public void Non_exsiting_projects() => new Slnx.OmitProjectIds()
        .ForInlineSlnx("""
<Solution>
    <Project Path=".net.csproj" Id="EB70735D-B797-4E9A-A508-A98BC21C0EDD" />
</Solution>
""")
        .HasIssue(Issue.WRN("Proj5005", "Remove the Project ID"));
}
