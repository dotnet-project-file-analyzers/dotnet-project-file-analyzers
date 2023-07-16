namespace DotNetProjectFile.MsBuild;

public sealed class Version : StringValueNode
{
    public Version(XElement element, Node parent, Project project) : base(element, parent, project) { }
}
