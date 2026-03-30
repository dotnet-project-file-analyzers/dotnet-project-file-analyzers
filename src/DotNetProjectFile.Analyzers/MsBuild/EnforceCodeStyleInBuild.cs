namespace DotNetProjectFile.MsBuild;

public sealed class EnforceCodeStyleInBuild(XElement element, Node parent, MsBuildProject project)
    : Node<bool?>(element, parent, project)
{ }
