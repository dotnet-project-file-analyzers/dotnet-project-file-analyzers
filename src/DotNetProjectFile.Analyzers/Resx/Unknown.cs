namespace DotNetProjectFile.Resx;

[DebuggerDisplay("{Element}")]
public sealed class Unknown(XElement element, Resource? resource)
    : Node(element, resource) { }
