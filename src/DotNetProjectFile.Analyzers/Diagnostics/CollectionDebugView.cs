namespace DotNetProjectFile.Diagnostics;

/// <summary>Allows the debugger to display collections.</summary>
[ExcludeFromCodeCoverage]
internal sealed class CollectionDebugView(IEnumerable enumeration)
{
    /// <summary>The array that is shown by the debugger.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public ImmutableArray<object> Items => [.. enumeration.Cast<object>()];

    /// <summary>A reference to the enumeration to display.</summary>
    private readonly IEnumerable enumeration = enumeration;
}
