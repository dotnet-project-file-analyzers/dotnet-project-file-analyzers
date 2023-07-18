namespace DotNetProjectFile.MsBuild;

public sealed class Otherwise : Node
{
    public Otherwise(XElement element, Node parent, MsBuildProject project)
        : base(element, parent, project)
    {
        var whens = parent.Children;
        
        Condition = $"!({string.Join(" And ", parent.Children.OfType<When>().Select(w => w.Condition))})";
    }

    public override string Condition { get; }
}
