using DotNetProjectFile.IO;

namespace IO_specs;

public class IO_Path
{
    [TestCase("TEMP/my_file.cs", "my_file.cs", null)]
    [TestCase("TEMP/my_file.cs", "./my_file.cs", null)]
    [TestCase("TEMP/MY_FILE.cs", "./my_file.cs", "MY_FILE.cs")]
    [TestCase("TEMP/test/path/my_file.cs", "./test/../TEST/Path/My_file.cs", "test/path/my_file.cs")]
    public void CaseCompare(string file, string selector, string? compare)
        => IOPath.CaseCompare(IOFile.Parse(file), IOFile.Parse(selector)).Should().Be(compare);

#if Is_Windows
    [Test]
    public void has_backslash_separator()
        => IOPath.Separator.Should().Be('\\');

    [Test]
    public void Is_case_insensitive_on_windows()
        => IOPath.IsCaseSensitive.Should().BeFalse();
#else
    [Test]
    public void has_forwardslash_separator()
        => IOPath.Separator.Should().Be('/');

    [Test]
    public void Is_case_sensitive()
        => IOPath.IsCaseSensitive.Should().BeTrue();
#endif
}
