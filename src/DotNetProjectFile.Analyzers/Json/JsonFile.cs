using DotNetProjectFile.CodeAnalysis;
using Grammr;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static DotNetProjectFile.Json.JsonFileParser;

namespace DotNetProjectFile.Json;

/// <summary>Represents a JSON file.</summary>
public sealed class JsonFile(int count, GrammrTree tree)
    : GrammrNode(new(0, count), tree)
    , ProjectFile
{
    public JsonValue? Value => field ??= Children.OfType<JsonValue>().FirstOrDefault();

    /// <inheritdoc />
    public IOFile Path => SourceTree.Path;

    /// <inheritdoc />
    public SourceText Text => SourceTree.SourceText;

    /// <inheritdoc />
    public WarningPragmas WarningPragmas => WarningPragmas.None;

    public override IEnumerable<Diagnostic> GetDiagnostics() =>
    [
        .. base.GetDiagnostics(),
        .. Tokens
            .WhereOfKind(Kind.Unparsable)
            .SelectMany(t => Issue(Rule.Json.Invalid, t, $"'{Formatter.Format(t.Span[0])}' is unexpected")),
    ];

    public static JsonFile? Load(IOFile file)
    {
        try { return Parse(GrammrTree.Load(file)); }
        catch { return null; }
    }

    public static JsonFile? Load(AdditionalText text)
    {
        try { return Parse(GrammrTree.Load(text)); }
        catch { return null; }
    }

    public static JsonFile Parse(GrammrTree tree)
        => JsonFileParser.Parse(tree);
}
