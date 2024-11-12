using DotNetProjectFile.Git;
using SyntaxTree = DotNetProjectFile.Syntax.SyntaxTree;

namespace Benchmarks;

public class GitIgnoreFile
{
    private static readonly string Root = string.Join("/", Enumerable.Repeat("..", 7)) + "/Files/";

    private readonly List<SyntaxTree> Trees = [];

    public GitIgnoreFile() : this(Root) { }

    public GitIgnoreFile(string root)
    {
        string[] files = ["gitignore-050-lines.txt", "gitignore-231-lines.txt"];
        foreach (var file in files)
        {
            using var stream = new FileStream(root + file, FileMode.Open, FileAccess.Read);
            Trees.Add(SyntaxTree.Load(stream));
        }
    }

    [Params(0, 1)]
    public int Index { get; set; }

    [Benchmark]
    public GitIgnoreSyntax Parse() => GitIgnoreSyntax.Parse(Trees[Index]);
}
