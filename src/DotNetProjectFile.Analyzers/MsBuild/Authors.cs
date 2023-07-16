namespace DotNetProjectFile.MsBuild;

public sealed class Authors : StringValueNode
{
    public Authors(XElement element, Node parent, Project project) : base(element, parent, project) { }
}
