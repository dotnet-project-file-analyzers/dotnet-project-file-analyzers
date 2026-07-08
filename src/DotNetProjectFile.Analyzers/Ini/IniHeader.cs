using DotNetProjectFile.Collections;
using Grammr;
using static DotNetProjectFile.Ini.IniFileParser;

namespace DotNetProjectFile.Ini;

public sealed class IniHeader(SliceSpan span, GrammrTree tree) : GrammrNode(span, tree)
{
    public string? Text => field ??= Tokens.FirstOrDefault(t => t.Kind is Kind.HeaderToken).ToString()[1..^1];
}
