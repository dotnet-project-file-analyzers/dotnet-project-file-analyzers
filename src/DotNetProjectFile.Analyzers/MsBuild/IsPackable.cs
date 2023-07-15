namespace DotNetProjectFile.MsBuild;

public sealed class IsPackable : Node<bool?>
{
    public IsPackable(XElement element, Node parent, Project project) : base(element, parent, project) { }

    public override bool? Value => Convert<bool?>(Element.Value);
}
