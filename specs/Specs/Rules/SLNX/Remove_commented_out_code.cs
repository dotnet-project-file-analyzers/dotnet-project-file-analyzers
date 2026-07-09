namespace Rules.SLNX.Remove_commented_out_code;

public class Reports
{
    [Test]
    public void commented_out_XML() => new Slnx.RemoveCommentedOutCode()
        .ForInlineSlnx("""
        <Solution>
          <!-- .NET SDK only -->
          <Project Path=".net.csproj" />
          <!-- Project Path="src/SolutionFile.csproj" /-->
        </Solution>
        """)
        .HasIssue(Issue.WRN("Proj3002", "Remove the commented-out code")
            .WithSpan(03, 06, 03, 47));
}
