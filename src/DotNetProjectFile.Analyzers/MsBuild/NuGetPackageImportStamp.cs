namespace DotNetProjectFile.MsBuild;

public sealed class NuGetPackageImportStamp(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project);
