using DotNetProjectFile.Collections;
using Grammr;
using static DotNetProjectFile.Ini.IniFileParser;

namespace DotNetProjectFile.Ini;

public sealed class IniValue(SliceSpan span, GrammrTree tree) : GrammrNode(span, tree)
{
    public string? Text => field ??= Tokens.FirstOrNone(t => t.Kind is Kind.ValueToken)?.ToString();
}
