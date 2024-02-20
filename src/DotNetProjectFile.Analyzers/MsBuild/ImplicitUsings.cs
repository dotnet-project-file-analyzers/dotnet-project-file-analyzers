namespace DotNetProjectFile.MsBuild;

public sealed class ImplicitUsings(XElement element, Node parent, Project project)
    : Node<ImplicitUsings.Kind?>(element, parent, project)
{
    public enum Kind
    {
        disable,
        enable,
        @true,
    }
}
