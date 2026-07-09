using DotNetProjectFile.Collections;
using Grammr;

namespace DotNetProjectFile.Git;

public sealed class GitIgnoreEntry(SliceSpan span, GrammrTree tree) : GrammrNode(span, tree)
{
    public Glob? Glob { get; init; }
}
