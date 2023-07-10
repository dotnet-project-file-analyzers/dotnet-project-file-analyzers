using System.Xml.Linq;

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
public sealed class ItemGroup : Node
{
    /// <summary>Initializes a new instance of the <see cref="ItemGroup"/> class.</summary>
    /// <param name="element">
    /// The corresponding <see cref="XElement"/>.
    /// </param>
    public ItemGroup(XElement element, Project project) : base(element, project) { }

    /// <summary>Gets the child package references.</summary>
    public Nodes<PackageReference> PackageReferences => Children<PackageReference>();

    /// <summary>Gets the child folders.</summary>
    public Nodes<Folder> Folders => Children<Folder>();
}
