namespace DotNetProjectFile.MsBuild;

public sealed class PackageReference(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project)
{
    public string? Include => Attribute();

    public string? Update => Attribute();

    public string? Version => Attribute();

    public string? VersionOverride => Attribute();

    public string IncludeOrUpdate => Include ?? Update ?? string.Empty;

    public string VersionOrVersionOverride => Version ?? VersionOverride ?? string.Empty;

    public string? PrivateAssets => Attribute() ?? Child();

    /// <summary>Resolves the version taking CPM into account.</summary>
    public string? ResolveVersion()
        => Project.ManagePackageVersionsCentrally() is true
            ? VersionOverride ?? Project
            .WalkBackward()
            .OfType<PackageVersion>()
            .FirstOrDefault(v => v.Include == IncludeOrUpdate)?.Version
        : Version;
}
