using DotNetProjectFile.Collections;
using Grammr;
using static DotNetProjectFile.Ini.IniFileParser;

namespace DotNetProjectFile.Ini;

public sealed class IniEntry(SliceSpan span, GrammrTree tree) : GrammrNode(span, tree)
{
    public IniKey? Key => field ??= Children.OfType<IniKey>().SingleOrDefault();

    public IniValue? Value => field ??= Children.OfType<IniValue>().SingleOrDefault();

    /// <inheritdoc />
    public override IEnumerable<Diagnostic> GetDiagnostics() => this switch
    {
        _ when Tokens.WhereOfKind(Kind.EqualToken).None()
            && Tokens.WhereOfKind(Kind.ColonToken).None()
            => [Issue(Rule.Ini.InvalidKeyValuePair, Tokens.OfKind(Kind.KeyToken).NextChar(), "'=' or ':' is expected")],

        _ when Value is null
            => [Issue(Rule.Ini.InvalidKeyValuePair, Tokens.OfAnyKind(Kind.EqualToken, Kind.ColonToken).NextChar(), "Value is expected")],

        _ => [],
    };
}
