using DotNetProjectFile.Analyzers.Files;

namespace Rules.Generic.Only_use_UTF8_without_BOM;

public class Reports
{
    [Test]
    public void files_with_UTF8_BOM()
       => new OnlyUseUTF8WithoutBom()
       .ForProject("MultipleEncodings.cs")
       .HasIssues(
           new Issue("Proj3000", "This file is using UTF-8 encoding with BOM.").WithPath("utf8-bom.css"),
           new Issue("Proj3000", "This file is using UTF-8 encoding with BOM.").WithPath("file.utf8-bom"));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project)
         => new OnlyUseUTF8WithoutBom()
        .ForProject(project)
        .HasNoIssues();
}
