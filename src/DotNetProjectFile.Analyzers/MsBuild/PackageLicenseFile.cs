namespace DotNetProjectFile.MsBuild;

public sealed class PackageLicenseFile(XElement element, Node parent, MsBuildProject project)
    : Node<string>(element, parent, project) { }
