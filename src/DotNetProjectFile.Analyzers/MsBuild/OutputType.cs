namespace DotNetProjectFile.MsBuild;

public sealed class OutputType(XElement element, Node parent, MsBuildProject project)
    : Node<OutputType.Kind?>(element, parent, project)
{
    public enum Kind
    {
        Library = 0,
        Exe = 1,
        Module = 2,
        WinExe = 3,
        WinMdObj = 4,
        AppContainerExe = 5,
    }
}
