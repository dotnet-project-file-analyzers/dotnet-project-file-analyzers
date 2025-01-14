using DotNetProjectFile.Collections;
using Microsoft.CodeAnalysis.Text;

namespace Grammr.Text;

/// <summary>Represents an append-only token stream.</summary>
[DebuggerTypeProxy(typeof(CollectionDebugView))]
[DebuggerDisplay("Count = {Count}, Remaining = {Remaining}")]
public readonly struct TokenStream : IReadOnlyList<SourceSpanToken>, IEquatable<TokenStream>
{
    private readonly AppendOnlyList<Info> Items;
    private readonly Source Source;

    internal static readonly TokenStream None = new(AppendOnlyList<Info>.Empty, Source.From(SourceText.From(string.Empty)));

    [Pure]
    public static TokenStream From(string sourceText) => new(AppendOnlyList<Info>.Empty, Source.From(sourceText));

    [Pure]
    public static TokenStream From(SourceText sourceText) => new(AppendOnlyList<Info>.Empty, Source.From(sourceText));

    private TokenStream(AppendOnlyList<Info> items, Source source)
    {
        Items = items;
        Source = source;
    }

    /// <inheritdoc />
    public SourceSpanToken this[int index]
    {
        get
        {
            var info = Items[index];
            return new SourceSpanToken(new(Source, info.TextSpan), info.Kind);
        }
    }

    /// <summary>Gets the number of tokens in the stream.</summary>
    public int Count => Items.Count;

    /// <summary>Gets the Length of the stream.</summary>
    public int Length => Count == 0 ? 0 : Items[^1].TextSpan.End;

    /// <summary>The remainder of the source that is not reflaxted (yet) in a token(s).</summary>
    public SourceSpan Remaining
    {
        get
        {
            if (Count == 0)
            {
                return Source;
            }
            var end = Length;
            return new SourceSpan(Source, new(end, Source.Length - end));
        }
    }

    /// <summary>Gets the text of the underlying source text.</summary>
    public string Text => Source.ToString();

    /// <summary>Adds a new token to the stream.</summary>
    public TokenStream Add(TextSpan span, string? kind) => new(Items.Add(new Info(span, kind)), Source);

    /// <inheritdoc />
    [Pure]
    public override bool Equals(object? obj) => obj is TokenStream other && Equals(other);

    /// <inheritdoc />
    [Pure]
    public bool Equals(TokenStream other)
        => Items.Count == other.Items.Count
        && Items[^1].TextSpan == other.Items[^1].TextSpan
        && Enumerable.SequenceEqual(Items, other.Items);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode()
    {
        var hash = Items.Count;
        foreach (var item in Items)
        {
            hash ^= (17 * hash) ^ item.GetHashCode();
        }
        return hash;
    }

    /// <inheritdoc />
    [Pure]
    public IEnumerator<SourceSpanToken> GetEnumerator()
    {
        var sourceText = Source;

        return Items
            .Select(info => new SourceSpanToken(new(sourceText, info.TextSpan), info.Kind))
            .GetEnumerator();
    }

    /// <inheritdoc />
    [Pure]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private readonly record struct Info(TextSpan TextSpan, string? Kind);
}
