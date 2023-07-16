namespace DotNetProjectFile.MsBuild;

public sealed class OutputType : Node<OutputType.Kind?>
{
    public enum Kind
    {
        Library,
        Exe,
        Module,
        WinExe,
    }

    public OutputType(XElement element, Node parent, Project project) : base(element, parent, project) { }
}
