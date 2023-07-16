namespace DotNetProjectFile.MsBuild;

public sealed class Copyright : StringValueNode
{
    public Copyright(XElement element, Node parent, Project project) : base(element, parent, project) { }
}
