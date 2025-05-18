namespace DotNetProjectFile.MsBuild;

public sealed class NugetAuditMode(XElement element, Node parent, MsBuildProject project)
    : Node<NugetAuditMode.Kind?>(element, parent, project)
{
    public enum Kind
    {
        Direct,
        All,
    }
}
