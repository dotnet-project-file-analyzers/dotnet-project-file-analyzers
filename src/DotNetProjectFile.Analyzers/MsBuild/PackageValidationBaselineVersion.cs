namespace DotNetProjectFile.MsBuild;

public sealed class PackageValidationBaselineVersion(XElement element, Node parent, MsBuildProject project)
    : Node<string>(element, parent, project) { }
