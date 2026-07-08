using Grammr;
using Microsoft.CodeAnalysis.Text;
using static DotNetProjectFile.Ini.IniFileParser;

namespace DotNetProjectFile.Ini;

/// <summary>Represents an INI file.</summary>
public sealed class IniFile(int count, GrammrTree tree)
    : GrammrNode(new(0, count), tree)
    , ProjectFile
{
    /// <summary>Gets the INI sections.</summary>
    public GrammrNodes<IniSection> Sections => new(Children);

    /// <inheritdoc />
    public IOFile Path => SourceTree.Path;

    /// <inheritdoc />
    public SourceText Text => SourceTree.SourceText;

    /// <inheritdoc />
    public WarningPragmas WarningPragmas => WarningPragmas.None;

    public bool IsRoot
        => Sections.FirstOrDefault() is { } section
        && section.Header is null
        && section.Kvps
            .Where(kvp => kvp.Key.IsMatch("root"))
            .Select(kvp => kvp.Value.IsMatch("true"))
            .LastOrDefault();

    public override IEnumerable<Diagnostic> GetDiagnostics() =>
    [
        .. base.GetDiagnostics(),
        .. Tokens.WhereOfKind(Kind.Unparsable).Select(t => Issue(Rule.Ini.Invalid, t, $"'{Formatter.Format(t.Span[0])}' is unexpected")),
    ];

    public static IniFile? Load(IOFile file)
    {
        try { return Parse(GrammrTree.Load(file)); }
        catch { return null; }
    }

    public static IniFile? Load(AdditionalText text)
    {
        try { return Parse(GrammrTree.Load(text)); }
        catch { return null; }
    }

    public static IniFile Parse(GrammrTree tree)
        => IniFileParser.Parse(tree);
}
