namespace Rules.SLNX.File_should_exist;

public class Reports
{
    [Test]
    public void non_existing_files() => new Slnx.FileShouldExist().ForInlineSlnx("""
        <Solution>
          <Folder Name="/Solution Items/">
            <File Path="non-existing.file" />
            <File Path="C:/dir/other.file" />
            <!-- .net.csproj should exist on disk -->
            <File Path=".net.csproj" />
          </Folder>
          <Project Path="src/SolutionFile.csproj" />
        </Solution>
        """)
        .HasIssues(
            Issue.WRN("Proj5006", "Included file 'non-existing.file' does not exist").WithSpan(02, 04, 02, 37),
            Issue.WRN("Proj5006", "Included file 'C:/dir/other.file' does not exist").WithSpan(03, 04, 03, 37));
}
