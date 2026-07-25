using DotNetProjectFile.NuGet;

namespace DotNetProjectFile.MsBuild;

public sealed class PackageReference(XElement element, Node parent, MsBuildProject project)
    : PackageReferenceBase(element, parent, project)
{
    public string? VersionOverride => Attribute();

    public string VersionOrVersionOverride => Version ?? VersionOverride ?? string.Empty;

    public string? PrivateAssets => Attribute() ?? Child();

    public override PackageVersionInfo Info => new(IncludeOrUpdate, VersionOrVersionOverride);

    public override (Node Node, string Version)? ResolveVersionVerbose(bool cpmEnabled) => cpmEnabled switch
    {
        true when VersionOverride is { Length: > 0 } fixedVersion
            => (this, fixedVersion),

        true when WalkVersionOverrides() is { } versionOverride
            => versionOverride,

        true when WalkPackageVersions() is { } packageVersion
            => packageVersion,

        _ when Version?.Length > 0
            => (this, Version),

        _ => null,
    };

    private (Node Node, string Version)? WalkVersionOverrides()
        => Project
            .WalkBackward()
            .OfType<PackageReference>()
            .FirstOrDefault(p => p.IncludeOrUpdate == IncludeOrUpdate)
        is { VersionOverride.Length: > 0 } node
        ? (node, node.VersionOverride)
        : null;

    private (Node Node, string Version)? WalkPackageVersions()
        => Project
            .WalkBackward()
            .OfType<PackageVersion>()
            .FirstOrDefault(v => v.Include == IncludeOrUpdate)
        is { Version.Length: > 0 } node
        ? (node, node.Version)
        : null;
}
