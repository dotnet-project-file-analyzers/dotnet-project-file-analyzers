using DotNetProjectFile.Parsing;
using DotNetProjectFile.Syntax;

namespace DotNetProjectFile.Ini;

[DebuggerDisplay("{DebuggerDisplay}")]
public sealed record OldSectionSyntax : OldIniSyntax
{
    public OldHeaderSyntax? Header => Children.FirstOrDefault() as OldHeaderSyntax;

    public SyntaxNodeCollection<OldKeyValuePairSyntax> KeyValuePairs => new(this);

    public IEnumerable<KeyValuePair<string, string>> Kvps => KeyValuePairs
        .Where(kvp => kvp.GetDiagnostics().None())
        .Select(kvp => kvp.Kvp)
        .OfType<KeyValuePair<string, string>>();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    protected override string DebuggerDisplay => $"Syntax = Section, KVP's = {KeyValuePairs.Count}";

    public override IEnumerable<Diagnostic> GetDiagnostics() => Children.SelectMany(c => c.GetDiagnostics());

    public static OldIniSyntax New(Parser parser)
    {
        var ini = parser.Syntax as OldIniFileSyntax ?? new OldIniFileSyntax { Children = [new OldSectionSyntax()] };

        return ini with
        {
            Children = ini.Sections.WithLast(s => s with
            {
                Span = s.Span + parser.Span,
            }),
        };
    }
}
