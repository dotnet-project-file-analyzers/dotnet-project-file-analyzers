namespace DotNetProjectFile.MsBuild;

public sealed class TargetFramework : Node<string>
{
    public TargetFramework(XElement element, Node parent, Project project) : base(element, parent, project) { }
}
