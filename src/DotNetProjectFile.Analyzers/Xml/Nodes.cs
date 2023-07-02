using System.Globalization;

namespace DotNetProjectFile.Xml;

/// <summary>Represents a collection of <see cref="Node"/>s.</summary>
[DebuggerTypeProxy(typeof(Diagnostics.CollectionDebugView))]
[DebuggerDisplay("{DebuggerDisplay}")]
public sealed class Nodes<T> : IEnumerable<T> where T : Node
{
    /// <summary>Initializes a new instance of the <see cref="Nodes{T}"/> class.</summary>
    /// <param name="parent">
    /// The parent <see cref="Node"/>.
    /// </param>
    public Nodes(Node parent)
    {
        Parent = parent;
    }

    /// <summary>Gets the number of items.</summary>
    public int Count => Items.Count();

    /// <summary>Gets the specified item.</summary>
    /// <param name="index">
    /// The index of the item.
    /// </param>
    public T this[int index] => Items.Skip(index).FirstOrDefault();

    /// <summary>The parent node.</summary>
    private Node Parent { get; set; }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [ExcludeFromCodeCoverage/* Justification = "Debug experience only." */]
    private string DebuggerDisplay
        => string.Format(CultureInfo.InvariantCulture, "{0} Count = {1}", typeof(T).Name, Count);

    /// <inheritdoc />
    public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>Helper property to select the needed children.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private IEnumerable<T> Items => Parent.GetAllChildren().Where(child => child is T).Cast<T>();
}
