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

    public OutputType(XElement element, Project project) : base(element, project) { }

    public Kind? Value => Convert<Kind?>(Element.Value);
}
