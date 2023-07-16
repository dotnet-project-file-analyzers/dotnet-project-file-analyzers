namespace DotNetProjectFile.MsBuild;

public sealed class RepositoryUrl : Node<string>
{
    public RepositoryUrl(XElement element, Node parent, Project project) : base(element, parent, project) { }
}
