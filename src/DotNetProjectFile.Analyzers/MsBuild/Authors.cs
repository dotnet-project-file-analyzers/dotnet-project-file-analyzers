namespace DotNetProjectFile.MsBuild;

public sealed class Authors : Node<string>
{
    public Authors(XElement element, Node parent, Project project) : base(element, parent, project) { }
}
