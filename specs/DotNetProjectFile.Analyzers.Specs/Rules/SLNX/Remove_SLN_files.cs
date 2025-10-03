using DotNetProjectFile.Analyzers.Slnx;

namespace Rules.SLNX.Remove_SLN_files;

public class Reports
{
    [Test]
    public void leftover_sln_file() => new UseSlnxFiles()
        .ForSdkProject("RemoveSlnFiles")
        .HasIssue(Issue.WRN("Proj5001", "Remove remove-sln-files.sln")
            .WithPath("remove-sln-files.sln"));
}
