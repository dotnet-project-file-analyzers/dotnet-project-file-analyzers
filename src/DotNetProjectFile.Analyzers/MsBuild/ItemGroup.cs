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
    : Node(element, parent, project)
{
    /// <summary>Gets the child build actions references.</summary>
    public Nodes<BuildAction> BuildActions => new(Children);

    /// <summary>Gets the child global project references.</summary>
    public Nodes<GlobalPackageReference> GlobalPackageReferences => new(Children);

    /// <summary>Gets the child package references.</summary>
    public Nodes<PackageReference> PackageReferences => new(Children);

    /// <summary>Gets the child package versions.</summary>
    public Nodes<PackageVersion> PackageVersions => new(Children);

    /// <summary>Gets the child project references.</summary>
    public Nodes<ProjectReference> ProjectReferences => new(Children);

    /// <summary>Gets the child project third-party licenses.</summary>
    public Nodes<ThirdPartyLicense> ThirdPartyLicenses => new(Children);

    /// <summary>Gets the child folders.</summary>
    public Nodes<Folder> Folders => new(Children);

    /// <summary>Gets the child project references.</summary>
    public Nodes<Using> Usings => new(Children);
}
