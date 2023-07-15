using System.Xml.Linq;

namespace DotNetProjectFile.MsBuild;

public sealed class OutputType : Node
{
    public enum Kind
    {
        Library,
        Exe,
        Module,
        WinExe,
    }

    public OutputType(XElement element, Node parent, Project project) : base(element, parent, project) { }

    public Kind? Value => Convert<Kind?>(Element.Value);
}
