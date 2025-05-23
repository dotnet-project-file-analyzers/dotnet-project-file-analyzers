namespace DotNetProjectFile.MsBuild;

public sealed class ContinuousIntegrationBuild(XElement element, Node parent, MsBuildProject project)
    : Node<bool?>(element, parent, project)
{ }
