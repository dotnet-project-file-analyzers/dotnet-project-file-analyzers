using DotNetProjectFile.Collections;
using System.Runtime.CompilerServices;

namespace Grammr;

[DebuggerTypeProxy(typeof(CollectionDebugView))]
[DebuggerDisplay("Count = {Count}, Start = {Start}, Span = {Span.ToString(),nq}")]
public readonly struct TokenStream : IReadOnlyList<Token>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Info[] Tokens;

    private TokenStream(int count, Info[] tokens, Source source)
    {
        Count = count;
        Tokens = tokens;
        Source = source;
    }

    private readonly Source Source;

    /// <inheritdoc />
    public int Count { get; }

    /// <summary>Gets the start index of the first token.</summary>
    public int Start => Count is 0 ? 0 : Tokens[0].Start;

    /// <summary>Gets the length of the span represented by the tokens.</summary>
    public int Length => Count is 0 ? 0 : Tokens[Count - 1].End - Tokens[0].Start;

    /// <summary>Gets the span repesented by the tokens.</summary>
    public ReadOnlySpan<char> Span => Source.AsSpan(Start, Length);

    /// <inheritdoc />
    public Token this[int index]
    {
        get
        {
            var info = Tokens[index];
            return new(info.Start, info.Length, Source, info.Kind);
        }
    }

    /// <summary>Gets the tokens specifiec by the range.</summary>
    public Slice<Token> this[SliceSpan range] => new(range, this);

    /// <summary>Creates a new stream with the token added.</summary>
    /// <param name="token">
    /// The token to add.
    /// </param>
    [Pure]
    public TokenStream Add(Token token)
        => token.Length is 0
        ? this
        : new(Count + 1, Add(token.Start, token.Length, token.Kind), Source);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Info[] Add(int start, int length, string? kind)
    {
        // If it still fits, and not branched.
        if (Count < Tokens.Length && Tokens[Count].IsDefault)
        {
            Tokens[Count] = new(start, length, kind);
            return Tokens;
        }
        else
        {
            var copy = new Info[(Count + 1) << 1];
            Array.Copy(Tokens, copy, Count);
            copy[Count] = new(start, length, kind);
            return copy;
        }
    }

    /// <inheritdoc cref="IEnumerable.GetEnumerator()" />
    [Pure]
    public Enumerator GetEnumerator() => new(this);

    /// <summary>Implicitly casts a <see cref="Grammr.Source"/> to a <see cref="TokenStream"/>.</summary>
    public static implicit operator TokenStream(Source source) => new(0, [], source);

    /// <inheritdoc />
    [Pure]
    [ExcludeFromCodeCoverage/*(Justification = "Only exists for backwards compatibillity reasons.")*/]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    [Pure]
    [ExcludeFromCodeCoverage/*(Justification = "Only exists for backwards compatibillity reasons.")*/]
    IEnumerator<Token> IEnumerable<Token>.GetEnumerator() => GetEnumerator();

    private readonly struct Info(int start, int length, string? kind)
    {
        public bool IsDefault => Length is 0;

        public string? Kind { get; } = kind;

        public int Start { get; } = start;

        public int Length { get; } = length;

        public int End => Start + Length;

        /// <inheritdoc />
        [Pure]
        public override string ToString() => $"[{Start}..{Start + Length}]";
    }

    public struct Enumerator : IEnumerator<Token>
    {
        private readonly TokenStream Stream;
        private int Index;

        internal Enumerator(TokenStream stream)
        {
            Stream = stream;
            Index = -1;
        }

        /// <inheritdoc />
        public readonly Token Current => Stream[Index];

        /// <inheritdoc />
        readonly object IEnumerator.Current => Current;

        /// <inheritdoc />
        public bool MoveNext() => ++Index < Stream.Count;

        /// <inheritdoc />
        readonly void IDisposable.Dispose() { /* Nothing to dispose. */ }

        /// <inheritdoc />
        [DoesNotReturn]
        void IEnumerator.Reset() => throw new NotSupportedException();
    }
}
