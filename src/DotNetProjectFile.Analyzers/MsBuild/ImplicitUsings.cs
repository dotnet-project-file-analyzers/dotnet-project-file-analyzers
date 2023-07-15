namespace DotNetProjectFile.MsBuild;

public sealed class ImplicitUsings : Node
{
    public enum Kind
    {
        disable,
        enable,
        @true,
    }

    public ImplicitUsings(XElement element, Node parent, Project project) : base(element, parent, project) { }

    public Kind? Value => Convert<Kind?>(Element.Value);
}
