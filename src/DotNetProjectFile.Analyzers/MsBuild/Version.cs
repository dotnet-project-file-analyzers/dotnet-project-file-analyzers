namespace DotNetProjectFile.MsBuild;

public sealed class Version : Node<string>
{
    public Version(XElement element, Node parent, Project project) : base(element, parent, project) { }
}
