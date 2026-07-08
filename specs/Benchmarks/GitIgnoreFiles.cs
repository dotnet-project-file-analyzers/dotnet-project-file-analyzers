using DotNetProjectFile.Git;
using Microsoft.CodeAnalysis.Text;
using TestData;

namespace Bench;

[MemoryDiagnoser(true)]
public class GitIgnoreFiles
{
	private static readonly SourceText git_0344 = Files.Text("gitignore-0344-lines.txt");
	private static readonly SourceText git_0606 = Files.Text("gitignore-0606-lines.txt");

    [Benchmark]
    public GitIgnoreFile _0344() => GitIgnoreFile.Parse(new(default, git_0344));

    [Benchmark]
    public GitIgnoreFile _0606() => GitIgnoreFile.Parse(new(default, git_0606));
}
