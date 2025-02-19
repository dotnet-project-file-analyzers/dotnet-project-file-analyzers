namespace DotNetProjectFile.MsBuild;

public sealed class PackageRequireLicenseAcceptance(XElement element, Node parent, MsBuildProject project)
    : Node<bool?>(element, parent, project) { }
