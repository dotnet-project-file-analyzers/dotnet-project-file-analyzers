using DotNetProjectFile.Analyzers.Files;

namespace Rules.Files.Use_UTF8_encoding;

public class Reports
{
    [Test]
    public void on_no_output_type()
       => new UseUTF8Encoding()
       .ForProject("MultipleEncodings.cs")
       .HasIssue(
           new Issue("Proj0900", "This file's encoding is not UTF-8 without BOM."));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project)
         => new UseUTF8Encoding()
        .ForProject(project)
        .HasNoIssues();
}
