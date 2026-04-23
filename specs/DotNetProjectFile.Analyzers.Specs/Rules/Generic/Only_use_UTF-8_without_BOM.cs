using DotNetProjectFile.Analyzers.Generic;
using System.IO;

namespace Rules.Generic.Only_use_UTF8_without_BOM;

public class DiagnosticdId
{
    [Test]
    public void Is_Proj3000() => new OnlyUseUTF8WithoutBom().Should().HaveId("Proj3000");
}

[Explicit]
public class Exists
{
    [TestCase(1)]
    [TestCase(2)]
    public void tiny_files(int byteSize)
    {
        using var stream = new MemoryStream();

        for (byte i = 1; i <= byteSize; i++)
        {
            stream.WriteByte(i);
        }

        using var file = new FileStream(
            $"../../../../../projects/MultipleEncodings/{byteSize}.byte",
            FileMode.Create,
            FileAccess.Write);

        stream.Position = 0;
        stream.CopyTo(file);
        file.Flush();

        var loc = new FileInfo(file.Name);
        Console.WriteLine(loc.FullName);

        loc.Exists.Should().BeTrue();
    }
}


public class Reports
{
    [Test]
    public void files_with_UTF8_BOM() => new OnlyUseUTF8WithoutBom()
        .ForProject("MultipleEncodings.cs")
        .HasIssues(
            Issue.WRN("Proj3000", "This file is using UTF-8 encoding with BOM").WithPath("utf8-bom.css"),
            Issue.WRN("Proj3000", "This file is using UTF-8 encoding with BOM").WithPath("file.utf8-bom"));
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
            .HasIssue(Issue.WRN("Proj3000", "This file is using UTF-8 encoding with BOM").WithPath("file.utf8-bom"));
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
