using System.Globalization;

namespace DotNetProjectFile.MsBuild;

/// <summary>Represents a collection of <see cref="Node"/>s.</summary>
/// <remarks>Initializes a new instance of the <see cref="Nodes{T}"/> class.</remarks>
[DebuggerTypeProxy(typeof(CollectionDebugView))]
[DebuggerDisplay("{DebuggerDisplay}")]
public sealed class Nodes<T>(IReadOnlyList<T> items) : IReadOnlyList<T> where T : Node
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly IReadOnlyList<T> Items = items;

    /// <summary>Gets the number of items.</summary>
    public int Count => Items.Count;

    /// <summary>Gets the specified item.</summary>
    /// <param name="index">
    /// The index of the item.
    /// </param>
    public T this[int index] => Items[index];

    public Nodes<TOut> Typed<TOut>() where TOut : T
        => new(Items.OfType<TOut>().ToArray());

    public Nodes<TOut> NestedTyped<TOut>() where TOut : T
        => new(Items.SelectMany(n => n.DescendantsAndSelf()).OfType<TOut>().ToArray());

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [ExcludeFromCodeCoverage/* Justification = "Debug experience only." */]
    private string DebuggerDisplay
        => string.Format(CultureInfo.InvariantCulture, "{0} Count = {1}", typeof(T).Name, Count);

    /// <inheritdoc />
    public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
