namespace DotNetProjectFile.MsBuild;

/// <summary>Represents an item definition group in a Visual Studio project file.</summary>
public sealed class ItemDefinitionGroup(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project);
