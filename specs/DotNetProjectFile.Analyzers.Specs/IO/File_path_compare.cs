using DotNetProjectFile.IO;

namespace IO.File_path_compare;

public class Compares
{
    [TestCase("/path/", "\\path\\")]
    [TestCase("/path/", "/path\\")]
    [TestCase("/path/", "/path/")]
    public void Same_paths_as_equal(string left, string right)
        => FileSystem.PathCompare.Compare(left, right).Should().Be(0);

    [Test]
    public void sub_strings_before()
        => new[] 
        {
            "substring",
            "sub",
        }
        .Should().BeInDescendingOrder(FileSystem.PathCompare);

    [Test]
    public void shorter_paths_first()
        => new[]
        {
            @"..\Root.AspNetCore\Root.AspNetCore.csproj",
            @"..\Root.AspNetCore.Builder.Abstractions\Root.AspNetCore.Builder.Abstractions.csproj",
        }
        .Should().BeInAscendingOrder(FileSystem.PathCompare);

    [Test]
    public void case_insensitive()
         => new[]
        {
            @"covlet",
            @"Qowaiv",
        }
        .Should().BeInAscendingOrder(FileSystem.PathCompare);
}
