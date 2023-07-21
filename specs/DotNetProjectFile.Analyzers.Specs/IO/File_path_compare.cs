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
        .OrderBy(x => x, FileSystem.PathCompare)
        .Should().BeEquivalentTo(
            "sub",
            "substring"
        );

    [Test]
    public void shorter_paths_first()
        => new[] 
        {
            @"..\Root.AspNetCore.Builder.Abstractions\Root.AspNetCore.Builder.Abstractions.csproj",
            @"..\Root.AspNetCore\Root.AspNetCore.csproj",
        }
        .OrderBy(x => x, FileSystem.PathCompare)
        .Should().BeEquivalentTo(
            @"..\Root.AspNetCore\Root.AspNetCore.csproj",
            @"..\Root.AspNetCore.Builder.Abstractions\Root.AspNetCore.Builder.Abstractions.csproj"
        );
}
