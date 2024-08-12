using DotNetProjectFile.IO;

namespace IO_specs;

public class IO_File
{
    [TestCase("/", "./root/test/somefile.txt")]
    [TestCase("\\", @".\root\test\somefile.txt")]
    public void Supports_formatting(string format, string formatted)
        => IOFile.Parse("./root/test/somefile.txt").ToString(format).Should().Be(formatted);

    [Test]
    public void Is_separator_agnostic()
        => IOFile.Parse(".\\root\\test\\somefile.txt").Should().Be(IOFile.Parse("./root/test/somefile.txt"));

    [TestCase(@"c:\Program Files\Microsoft.NET")]
    public void Supports_type_conversion(IOFile path)
        => path.Should().Be(IOFile.Parse(@"c:\Program Files\Microsoft.NET"));

    [TestCase("one\\test.txt", "one/test.txt")]
    [TestCase("one\\two/test.txt", "one/two/test.txt")]
    [TestCase("one/./test.txt", "one/test.txt")]
    [TestCase("../../test.txt", "../../test.txt")]
    [TestCase("./../../test.txt", "./../../test.txt")]
    [TestCase("./one/two/../../test.txt", "./test.txt")]
    [TestCase("one/../../test.txt", "../test.txt")]
    [TestCase("./one/../../test.txt", "./../test.txt")]
    public void normalizes_path(string path, string normalized)
        => IOFile.Parse(path).ToString("/").Should().Be(normalized);
}
