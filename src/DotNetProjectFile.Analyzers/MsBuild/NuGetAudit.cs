namespace DotNetProjectFile.MsBuild;

public sealed class NuGetAudit : Node
{
    public NuGetAudit(XElement element, Node parent, Project project) : base(element, parent, project) { }

    public bool? Value => Convert<bool?>(Element.Value);
}
