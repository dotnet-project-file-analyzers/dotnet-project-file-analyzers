namespace DotNetProjectFile.MsBuild;

public sealed class PropertyGroup(XElement element, Node parent, MsBuildProject project) : Node(element, parent, project);
