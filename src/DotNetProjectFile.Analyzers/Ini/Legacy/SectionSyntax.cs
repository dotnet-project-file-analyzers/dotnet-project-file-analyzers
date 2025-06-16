using DotNetProjectFile.Parsing;
using DotNetProjectFile.Syntax;

namespace DotNetProjectFile.Ini.Legacy;

[DebuggerDisplay("{DebuggerDisplay}")]
public sealed record SectionSyntax : IniSyntax
{
    public HeaderSyntax? Header => Children.FirstOrDefault() as HeaderSyntax;

    public SyntaxNodeCollection<KeyValuePairSyntax> KeyValuePairs => new(this);

    public IEnumerable<KeyValuePair<string, string>> Kvps => KeyValuePairs
        .Where(kvp => kvp.GetDiagnostics().None())
        .Select(kvp => kvp.Kvp)
        .OfType<KeyValuePair<string, string>>();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    protected override string DebuggerDisplay => $"Syntax = Section, KVP's = {KeyValuePairs.Count}";

    public override IEnumerable<Diagnostic> GetDiagnostics() => Children.SelectMany(c => c.GetDiagnostics());

    public static IniSyntax New(Parser parser)
    {
        var ini = parser.Syntax as IniFileSyntax ?? new IniFileSyntax { Children = [new SectionSyntax()] };

        return ini with
        {
            Children = ini.Sections.WithLast(s => s with
            {
                Span = s.Span + parser.Span,
            }),
        };
    }
}
