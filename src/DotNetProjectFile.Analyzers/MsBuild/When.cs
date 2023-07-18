namespace DotNetProjectFile.MsBuild;

public sealed class When : Node
{
    public When(XElement element, Node parent, MsBuildProject project) : base(element, parent, project) { }
}
