namespace DotNetProjectFile.MsBuild;

public sealed class DebugType(XElement element, Node parent, MsBuildProject project)
    : Node<DebugType.Kind?>(element, parent, project)
{
    public enum Kind
    {
        none,
        embedded,
        pdbonly,
        portable,
        full,
    }
}
