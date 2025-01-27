namespace DotNetProjectFile.MsBuild;

public sealed class EnableStrictModeForCompatibleFrameworksInPackage(XElement element, Node parent, MsBuildProject project)
    : Node<bool?>(element, parent, project) { }
