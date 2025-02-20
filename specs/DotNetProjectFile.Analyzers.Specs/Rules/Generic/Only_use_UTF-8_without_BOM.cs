using DotNetProjectFile.Analyzers.Generic;
using System.IO;

namespace Rules.Generic.Only_use_UTF8_without_BOM;

public class DiagnosticdId
{
    [Test]
    public void Is_Proj3000() => new OnlyUseUTF8WithoutBom().Should().HaveId("Proj3000");
}

public class Reports
{
    [Test]
    public void files_with_UTF8_BOM() => new OnlyUseUTF8WithoutBom()
        .ForProject("MultipleEncodings.cs")
        .HasIssues(
           Issue.WRN("Proj3000", "This file is using UTF-8 encoding with BOM.").WithPath("utf8-bom.css"),
           Issue.WRN("Proj3000", "This file is using UTF-8 encoding with BOM.").WithPath("file.utf8-bom"));
}

public class Does_not_crash
{
    [Test]
    [NonParallelizable] // Required due to file lock
    public void scanning_locked_files()
    {
        using var _ = new FileInfo("../../../../../projects/MultipleEncodings/utf8-bom.css").Lock();

        new OnlyUseUTF8WithoutBom()
            .ForProject("MultipleEncodings.cs")
            .HasIssue(Issue.WRN("Proj3000", "This file is using UTF-8 encoding with BOM.").WithPath("file.utf8-bom"));
    }
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new OnlyUseUTF8WithoutBom()
        .ForProject(project)
        .HasNoIssues();
}
