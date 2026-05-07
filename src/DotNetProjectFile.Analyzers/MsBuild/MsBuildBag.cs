namespace DotNetProjectFile.MsBuild;

/// <summary>Contains a bag of items.</summary>
[DebuggerTypeProxy(typeof(CollectionDebugView))]
[DebuggerDisplay("Count = {Count}")]
public sealed class MsBuildBag<TItem> : IReadOnlyCollection<KeyValuePair<string, TItem>>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Dictionary<string, TItem> Bag = new(StringComparer.OrdinalIgnoreCase);

    /// <inheritdoc />
    public int Count => Bag.Count;

    /// <summary>Includes the item in the bag.</summary>
    public bool Include(string key, TItem item)
    {
        if (Key(key) is { } key_)
        {
            Bag[key_] = item;
            return true;
        }
        else return false;
    }

    /// <summary>Updates the item if the key is already present.</summary>
    public bool Update(string key, TItem item)
    {
        if (Key(key) is { } key_ && Bag.ContainsKey(key_))
        {
            Bag[key_] = item;
            return true;
        }
        else return false;
    }

    /// <summary>Removes the key from the bag if it is present.</summary>
    public bool Remove(string key)
        => Key(key) is { } key_
        && Bag.Remove(key_);

    /// <summary>Trims the key.</summary>
    [Pure]
    private static string? Key(string? key)
        => (key ?? string.Empty).Trim() is { Length: > 0 } key_
        ? key_
        : null;

    /// <inheritdoc />
    public IEnumerator<KeyValuePair<string, TItem>> GetEnumerator() => Bag.GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
