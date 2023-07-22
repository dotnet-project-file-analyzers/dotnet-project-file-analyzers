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
    public ItemGroup(XElement element, Node parent, Project project) : base(element, parent, project)
    {
        PackageReferences = Children.Typed<PackageReference>();
        ProjectReferences = Children.Typed<ProjectReference>();
        Folders = Children.Typed<Folder>();
    }

    /// <summary>Gets the child package references.</summary>
    public Nodes<PackageReference> PackageReferences { get; }

    /// <summary>Gets the child project references.</summary>
    public Nodes<ProjectReference> ProjectReferences { get; }

    /// <summary>Gets the child folders.</summary>
    public Nodes<Folder> Folders { get; }
}
