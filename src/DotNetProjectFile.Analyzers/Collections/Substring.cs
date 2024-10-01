namespace DotNetProjectFile.Collections;

/// <summary>Represents a substring (or slice) of a string.</summary>
public readonly struct Substring : IEquatable<Substring>
{
    private Substring(string text, int start, int length)
    {
        Offset = start;
        Text = text;
        Length = length;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly int Offset;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly string Text;

    /// <summary>Gets the length of the substring.</summary>
    public readonly int Length;

    /// <summary>Gets the char of the substring at the specified index.</summary>
    public char this[int index] => Text[Offset + index];

    [Pure]
    public Substring Slice(int start, int length)
        => new(Text, Offset + start, length);

    [Pure]
    public Substring Slice(SliceSpan span) => Slice(span.Start, span.Size);

    /// <inheritdoc />
    [Pure]
    public override string ToString() => Text.Substring(Offset, Length);

    [Pure]
    public override bool Equals(object obj) => obj is Substring other && Equals(other);

    /// <inheritdoc />
    [Pure]
    public bool Equals(Substring other)
    {
        if (Length != other.Length)
        {
            return false;
        }
        for (var i = 0; i < Length; i++)
        {
            if (this[i] != other[i]) return false;
        }
        return true;
    }

    [Pure]
    public override int GetHashCode()
    {
        var hash = 0;
        for (var i = 0; i < Length; ++i)
        {
            hash *= 17;
            hash ^= this[i].GetHashCode();
        }
        return hash;
    }

    /// <summary>Implicitly casts a <see cref="string"/> to a <see cref="Substring" />.</summary>
    public static implicit operator Substring(string str) => new(str, 0, str.Length);
}
