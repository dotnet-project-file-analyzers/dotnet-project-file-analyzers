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

    public SourceSpan SourceSpan => new(SourceText, new(0, SourceText.Length));

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
    public static SyntaxTree From(Stream stream) => new()
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
