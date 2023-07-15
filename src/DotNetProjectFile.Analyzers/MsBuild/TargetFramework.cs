namespace DotNetProjectFile.MsBuild;

public sealed class TargetFramework : Node
{
    public TargetFramework(XElement element, Node parent, Project project) : base(element, parent, project) { }

    public string? Values => Element.Value;
}
