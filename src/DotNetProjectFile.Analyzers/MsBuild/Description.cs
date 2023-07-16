namespace DotNetProjectFile.MsBuild;

public sealed class Description : StringValueNode
{
    public Description(XElement element, Node parent, Project project) : base(element, parent, project) { }
}
