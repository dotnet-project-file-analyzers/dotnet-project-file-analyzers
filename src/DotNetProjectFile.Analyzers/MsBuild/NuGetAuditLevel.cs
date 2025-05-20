namespace DotNetProjectFile.MsBuild;

public sealed class NuGetAuditLevel(XElement element, Node parent, MsBuildProject project)
    : Node<NuGetAuditLevel.Kind?>(element, parent, project)
{
    public enum Kind
    {
        Low,
        Moderate,
        High,
        Critical,
    }
}
