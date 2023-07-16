namespace DotNetProjectFile.MsBuild;

public sealed class ImplicitUsings : Node<ImplicitUsings.Kind?>
{
    public enum Kind
    {
        disable,
        enable,
        @true,
    }

    public ImplicitUsings(XElement element, Node parent, Project project) : base(element, parent, project) { }
}
