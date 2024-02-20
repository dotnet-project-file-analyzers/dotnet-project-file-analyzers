namespace DotNetProjectFile.MsBuild;

public sealed class OutputType(XElement element, Node parent, Project project)
    : Node<OutputType.Kind?>(element, parent, project)
{
    public enum Kind
    {
        Library,
        Exe,
        Module,
        WinExe,
    }
}
