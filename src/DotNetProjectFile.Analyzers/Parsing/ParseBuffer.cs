using DotNetProjectFile.Syntax;
using System.Runtime.CompilerServices;

namespace DotNetProjectFile.Parsing;

/// <summary>This is an append-only collection.</summary>
/// <remarks>
/// This buffer is not thread-safe.
/// As this buffer should eventually lead to one fully parsed token, it does
/// attempt to keep all branches valid. Once a new branch is created, it will
/// reassign slots. This is safe for this use case, but could lead to unintended
/// results in other cases.
/// </remarks>
[DebuggerTypeProxy(typeof(CollectionDebugView))]
[DebuggerDisplay("Count = {Count}")]
public readonly struct ParseBuffer(int count, SourceSpanToken[] buffer) : IReadOnlyList<SourceSpanToken>
{
    public static readonly ParseBuffer Empty = new(0, []);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly SourceSpanToken[] Buffer = buffer;

    /// <remarks>Internal from .NET.</remarks>
    private const int MaxCapacity = 0X7FFFFFC7;

    /// <inheritdoc />
    public int Count { get; } = count;

    /// <inheritdoc />
    public SourceSpanToken this[int index] => Buffer[index];

    [Pure]
    public ParseBuffer Add(SourceSpanToken token)
    {
        var added = Ensure();
        added[Count] = token;
        return new(Count + 1, added);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private SourceSpanToken[] Ensure()
    {
        if (Count < Buffer.Length && Count != 0)
        {
            return Buffer;
        }
        int capacity = Buffer.Length == 0 ? 8 : 2 * Buffer.Length;

        // Allow the list to grow to maximum possible capacity (~2G elements) before encountering overflow.
        // Note that this check works even when _items.Length overflowed thanks to the (uint) cast
        if ((uint)capacity > MaxCapacity) capacity = MaxCapacity;
        return Copy(capacity);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private SourceSpanToken[] Copy(int capacity)
    {
        var copy = new SourceSpanToken[capacity];
        Array.Copy(Buffer, copy, Count);
        return copy;
    }

    /// <inheritdoc />
    [Pure]
    public IEnumerator<SourceSpanToken> GetEnumerator() => Buffer.Take(Count).GetEnumerator();

    /// <inheritdoc />
    [Pure]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
