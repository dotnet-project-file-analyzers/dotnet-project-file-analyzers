using Microsoft.CodeAnalysis.Text;

namespace Grammr;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public sealed class GrammrTree(IOFile path, SourceText text)
{
    public SourceText SourceText { get; } = text;

    public IOFile Path { get; } = path;

    public TokenStream Stream { get; private set; }

    public bool Final { get; private set; }

    public void Finalize(TokenStream stream)
    {
        ThrowIfFinal();
        Stream = stream;
        Final = true;
    }

    public void ThrowIfFinal()
    {
        if (Final) throw new InvalidOperationException("Tree has already been finalized");
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => Path.HasValue
        ? $"Size = {SourceText.Length}, Tokens = {Stream.Count}, Path = {Path}"
        : $"Size = {SourceText.Length}, Tokens = {Stream.Count}";

    /// <summary>:Loads the source tree.</summary>
    /// <param name="file">
    /// The file location to load from.
    /// </param>
    [Pure]
    public static GrammrTree Load(IOFile file) => new(file, file.SourceText());

    /// <summary>:Loads the source tree.</summary>
    /// <param name="text">
    /// The additional text load from.
    /// </param>
    [Pure]
    public static GrammrTree Load(AdditionalText text) => new(IOFile.Parse(text.Path), text.GetText()!);
}
