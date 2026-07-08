using DotNetProjectFile.Git;

namespace Specs.Git.GitIgnoreFile_specs;

public class Parses
{
    [TestCase("gitignore-0001-lines.txt", 1)]
    [TestCase("gitignore-0005-lines.txt", 4)]
    [TestCase("gitignore-0344-lines.txt", 179)]
    [TestCase("gitignore-0606-lines.txt", 225)]
    public void Embedded(string file, int count)
    {
        var tree = Files.Tree(file);
        var ignore = GitIgnoreFile.Parse(tree);
        ignore.Entries.Should().HaveCount(count);
    }
}
