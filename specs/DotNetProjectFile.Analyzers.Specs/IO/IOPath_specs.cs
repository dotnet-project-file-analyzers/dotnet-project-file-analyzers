using DotNetProjectFile.IO;

namespace IO_specs;

public class IO_Path
{
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
