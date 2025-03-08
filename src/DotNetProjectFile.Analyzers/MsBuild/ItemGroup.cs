namespace DotNetProjectFile.MsBuild;

/// <summary>Represents an item group in a Visual Studio project file.</summary>
/// <remarks>
/// Often, an item group only contains one type of children:
/// - Compile
/// - Content
/// - EmbeddedResource
/// - Folder
/// - Import
/// - None
/// - PackageReference
/// - ProjectReference
/// - Reference.
/// </remarks>
public sealed class ItemGroup(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project);
