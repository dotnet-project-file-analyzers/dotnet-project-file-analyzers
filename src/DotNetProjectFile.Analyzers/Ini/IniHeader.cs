using DotNetProjectFile.Collections;
using Grammr;
using static DotNetProjectFile.Ini.IniFileParser;

namespace DotNetProjectFile.Ini;

/// <summary>Represents the header (like: [*.csproj]) of an INI file.</summary>
public sealed class IniHeader(SliceSpan span, GrammrTree tree) : GrammrNode(span, tree)
{
    /// <summary>Gets the text of the INI header.</summary>
    public string? Text => field ??= Tokens.TryOfKind(Kind.HeaderText)?.ToString();

    /// <inheritdoc />
    public override IEnumerable<Diagnostic> GetDiagnostics() => this switch
    {
        _ when Tokens.HasNoneOfKind(Kind.HeaderText)
            => [Issue(Rule.Ini.InvalidHeader, Tokens.OfKind(Kind.HeaderStart).NextChar(), "header text is missing")],

        _ when Tokens.HasNoneOfKind(Kind.HeaderEnd)
            => [Issue(Rule.Ini.InvalidHeader, Tokens.OfKind(Kind.HeaderText).NextChar(), "']' is expected")],

        _ => [],
    };
}
