namespace DotNetProjectFile.MsBuild;

public sealed class RepositoryUrl : StringValueNode
{
    public RepositoryUrl(XElement element, Node parent, Project project) : base(element, parent, project) { }
}
