using DotNetProjectFile.Collections;
using Grammr;

namespace DotNetProjectFile.Ini;

public sealed class IniEntry(SliceSpan span, GrammrTree tree) : GrammrNode(span, tree)
{
    public IniKey? Key => field ??= Children.OfType<IniKey>().SingleOrDefault();

    public IniValue? Value => field ??= Children.OfType<IniValue>().SingleOrDefault();
}
