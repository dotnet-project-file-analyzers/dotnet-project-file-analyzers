using CodeAnalysis.TestTools;
using DotNetProjectFile.Analyzers;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Rules.Project_file_could_not_be_located;

public class Has_no_issues
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantVB.vb")]
    public void For(string project)
        => new ProjectFileCouldNotBeLocated()
        .ForProject(project)
        .HasNoIssues();
}
