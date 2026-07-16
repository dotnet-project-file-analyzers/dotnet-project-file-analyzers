using Grammr.Lexers;
using System.Text.RegularExpressions;

namespace Grammr;

/// <summary>A reader for a <see cref="Grammr.Source"/> that allows reading and advancing through the source.</summary>
[DebuggerDisplay("{DebuggerDisplay}")]
public ref struct SourceReader(Source source, TokenStream? stream = null)
{
    /// <summary>The source to read from.</summary>
    public Source Source { get; } = source;

    /// <summary>The token stream to write to, usage is optional.</summary>
    public TokenStream Stream { get; set; } = stream ?? source;

    /// <summary>Gets the remaining span of the source starting at the current position of the reader.</summary>
    public ReadOnlySpan<char> Span { get; private set; } = source.AsSpan(0, source.Length);

    /// <summary>Gets the start position of the reader in the source.</summary>
    public readonly int Start => Source.Length - Span.Length;

    /// <summary>Gets the remaining length of the source.</summary>
    public readonly int Length => Span.Length;

    /// <summary>Indicates if the reader has reached the end of the source.</summary>
    public readonly bool EOS => Span.Length == 0;

    /// <summary>Gets the character at the specified index in the source, relative to the current position of the reader.</summary>
    /// <param name="index">
    /// The index of the character to get.
    /// </param>
    /// <returns>
    /// The character at the specified index.
    /// </returns>
    public readonly char this[int index] => Span[index];

    /// <summary>Adds a token the stream.</summary>
    /// <param name="token">
    /// The token to add.
    /// </param>
    public SourceReader Add(Token token)
    {
        Stream = Stream.Add(token);
        return this;
    }

    /// <summary>Consumes the specified number of characters from the source.</summary>
    /// <param name="length">
    /// The length to consume.
    /// </param>
    /// <returns>
    /// True if the length could be consumed, otherwise false.
    /// </returns>
    public bool Cons(int length)
    {
        if (length <= Length)
        {
            Span = Span[length..];
            return true;
        }
        else return false;
    }

    /// <summary>Consumes if the lexers matches.</summary>
    /// <param name="lexer">
    /// The lexer to apply.
    /// </param>
    /// <returns>
    /// True if the lexer matched, otherwise false.
    /// </returns>
    public bool Cons(Lexer lexer)
    {
        if (lexer.Match(this) is { } length)
        {
            Span = Span[length..];
            return true;
        }
        else return false;
    }

    /// <summary>Consumes if the lexers matches.</summary>
    /// <param name="lexer">
    /// The lexer to apply.
    /// </param>
    /// <returns>
    /// The token describing the match.
    /// </returns>
    public Token? Take(Lexer lexer)
    {
        // To get the the matching lexer.Kind, we loop.
        if (lexer is Choice lexers)
        {
            for (var i = 0; i < lexers.Count; i++)
            {
                var choice = lexers[i];
                if (choice.Match(this) is { } len)
                {
                    var token = new Token(Start, len, Source, choice.Kind);
                    Span = Span[len..];
                    return token;
                }
            }
            return lexers.IsOptional ? default(Token) : null;
        }
        if (lexer.Match(this) is { } length)
        {
            var token = new Token(Start, length, Source, lexer.Kind);
            Span = Span[length..];
            return token;
        }
        else return null;
    }

    /// <summary>Consumes if the lexers matches, and emits it to the stream.</summary>
    /// <param name="lexer">
    /// The lexer to apply.
    /// </param>
    /// <returns>
    /// the consumed token, if any.
    /// </returns>
    public Token? Emit(Lexer lexer)
    {
        if (Take(lexer) is { } token)
        {
            Stream = Stream.Add(token);
            return token;
        }
        else return null;
    }

    /// <summary>Consumes if the lexers matches, and emits it to the stream.</summary>
    /// <param name="lexer">
    /// The lexer to apply.
    /// </param>
    /// <returns>
    /// True if the lexer matched, otherwise false.
    /// </returns>
    public bool Keep(Lexer lexer) => Emit(lexer) is not null;

    /// <summary>Matching the <see cref="Regex"/> pattern against the source starting at the current position.</summary>
    /// <param name="pattern">
    /// The pattern to match.
    /// </param>
    [Pure]
    public Match Match(Regex pattern) => Source.Match(pattern, Start);

    /// <inheritdoc />
    [Pure]
    public readonly override string ToString() => Span.ToString();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly string DebuggerDisplay => Formatter.Format(ToString());
}
