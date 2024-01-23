namespace DotNetProjectFile.MsBuild;

public sealed class PackageReleaseNotes(XElement element, Node parent, MsBuildProject project)
    : Node<string>(element, parent, project) { }
