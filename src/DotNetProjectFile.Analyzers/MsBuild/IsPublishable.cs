namespace DotNetProjectFile.MsBuild;

public sealed class IsPublishable : Node<bool?>
{
    public IsPublishable(XElement element, Node parent, Project project) : base(element, parent, project) { }
}
