namespace DotNetProjectFile.MsBuild;

public sealed class NuGetAuditMode(XElement element, Node parent, MsBuildProject project)
    : Node<NuGetAuditMode.Kind?>(element, parent, project)
{
    public enum Kind
    {
        Direct,
        All,
    }
}
