using Grammr;

namespace DotNetProjectFile.Git;

public sealed class GitIgnoreFile(int count, GrammrTree tree) : GrammrNode(new(0, count), tree)
{
    public GrammrNodes<GitIgnoreEntry> Entries => new(Children);

    public static GitIgnoreFile? Load(IOFile file)
    {
        try { return Parse(GrammrTree.Load(file)); }
        catch { return null; }
    }

    public static GitIgnoreFile? Load(AdditionalText text)
    {
        try { return Parse(GrammrTree.Load(text)); }
        catch { return null; }
    }

    public static GitIgnoreFile Parse(GrammrTree tree)
        => GitIgnoreFileParser.Parse(tree);
}
