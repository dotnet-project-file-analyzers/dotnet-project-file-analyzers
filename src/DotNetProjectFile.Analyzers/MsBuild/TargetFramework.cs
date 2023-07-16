namespace DotNetProjectFile.MsBuild;

public sealed class TargetFramework : StringValueNode
{
    public TargetFramework(XElement element, Node parent, Project project) : base(element, parent, project) { }
}
