namespace DotNetProjectFile.MsBuild;

public sealed class Otherwise(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project)
{
    public override string Condition => condition ??= GetCondtion();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string? condition;

    private string GetCondtion()
    {
        var whens = string.Join(" And ", Parent!.Children.OfType<Otherwise>().Select(o => $"({o.Condition})"));
        return $"!({whens})";
    }
}
