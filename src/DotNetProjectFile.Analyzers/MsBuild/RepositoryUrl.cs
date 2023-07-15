namespace DotNetProjectFile.MsBuild;

public sealed class RepositoryUrl : Node
{
    public RepositoryUrl(XElement element, Node parent, Project project) : base(element, parent, project) { }

    public string Value => Element.Value;
}
