namespace DotNetProjectFile.MsBuild;

public sealed class IsPackable : Node
{
    public IsPackable(XElement element, Node parent, Project project) : base(element, parent, project) { }

    public bool? Value => Convert<bool?>(Element.Value);
}
