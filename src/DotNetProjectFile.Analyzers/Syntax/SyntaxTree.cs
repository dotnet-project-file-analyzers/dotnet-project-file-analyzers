using Grammr.Text;
using Microsoft.CodeAnalysis.Text;
using System.IO;
using System.Text;

namespace DotNetProjectFile.Syntax;

[DebuggerDisplay("{DebuggerDisplay}")]
public sealed class SyntaxTree
{
    public IOFile Path { get; init; }

    public SourceText SourceText { get; init; } = SourceText.From(string.Empty);

    public Encoding? Encoding => SourceText.Encoding;

    public IReadOnlyList<SourceSpanToken> Tokens { get; init; } = [];

    public SourceSpan SourceSpan => Source.From(SourceText);

    /// <summary>Gets the location of the line span.</summary>
    [Pure]
    public Location GetLocation(LinePositionSpan lineSpan)
        => Location.Create(Path.ToString(), SourceText.TextSpan(lineSpan), lineSpan);

    public SyntaxTree With(IReadOnlyList<SourceSpanToken> tokens) => new()
    {
        Path = Path,
        SourceText = SourceText,
        Tokens = tokens,
    };

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => Path.HasValue
        ? $"Size = {SourceText.Length}, Tokens = {Tokens.Count}, Path = {Path}"
        : $"Size = {SourceText.Length}, Tokens = {Tokens.Count}";

    [Pure]
    public static SyntaxTree From(AdditionalText text) => new()
    {
        Path = IOFile.Parse(text.Path),
        SourceText = text.GetText()!,
    };

    [Pure]
    public static SyntaxTree Load(Stream stream) => new()
    {
        Path = stream is FileStream file ? IOFile.Parse(file.Name) : default,
        SourceText = SourceText.From(stream),
    };

    [Pure]
    public static SyntaxTree Parse(string text) => new()
    {
        SourceText = SourceText.From(text),
    };
}
