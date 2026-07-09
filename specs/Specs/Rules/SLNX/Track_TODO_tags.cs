namespace Rules.SLNX.Track_TODO_tags;

public class Reports
{
    [Test]
    public void TODO_tags() => new Slnx.TrackToDoTags()
        .ForInlineSlnx("""
        <Solution>
          <Folder Name="/Solution Items/">
            <!-- TODO: Include as project -->
            <File Path=".net.csproj" />
          </Folder>
          <Project Path="src/SolutionFile.csproj" />
        </Solution>
        """)
        .HasIssue(Issue.WRN("Proj3001", """Complete the task associated to this "TODO" comment""")
            .WithSpan(02, 08, 02, 34));
}
