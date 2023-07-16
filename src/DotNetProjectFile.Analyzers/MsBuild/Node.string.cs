namespace DotNetProjectFile.MsBuild;

public abstract class StringValueNode : Node<string>
{
    protected StringValueNode(XElement element, Node parent, Project project) : base(element, parent, project) { }

    public sealed override string Value => Element.Value;
}
