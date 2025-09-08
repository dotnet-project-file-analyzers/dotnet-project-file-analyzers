namespace DotNetProjectFile.MsBuild;

public sealed class OutputType(XElement element, Node parent, MsBuildProject project)
    : Node<OutputType.Kind?>(element, parent, project)
{
    public enum Kind
    {
        Library,
        Exe,
        Module,
        WinExe,
        WinMdObj,
        AppContainerExe,
    }
}
