namespace DotNetProjectFile.MsBuild;

public sealed class RestorePackagesWithLockFile(XElement element, Node parent, MsBuildProject project)
    : Node<bool?>(element, parent, project)
{ }
