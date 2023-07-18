namespace DotNetProjectFile.MsBuild;

public sealed class Choose : Node
{
    public Choose(XElement element, Node parent, MsBuildProject project) : base(element, parent, project) { }
}
