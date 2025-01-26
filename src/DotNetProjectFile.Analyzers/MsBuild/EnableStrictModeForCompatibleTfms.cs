namespace DotNetProjectFile.MsBuild;

public sealed class EnableStrictModeForCompatibleTfms(XElement element, Node parent, MsBuildProject project)
    : Node<bool?>(element, parent, project) { }
