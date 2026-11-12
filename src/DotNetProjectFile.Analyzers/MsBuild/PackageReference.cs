using DotNetProjectFile.NuGet;

namespace DotNetProjectFile.MsBuild;

public sealed class PackageReference(XElement element, Node parent, MsBuildProject project)
    : PackageReferenceBase(element, parent, project)
{
    public string? VersionOverride => Attribute();

    public string VersionOrVersionOverride => Version ?? VersionOverride ?? string.Empty;

    public string? PrivateAssets => Attribute() ?? Child();

    public override PackageVersionInfo Info => new(IncludeOrUpdate, VersionOrVersionOverride);

    public override (Node Node, string Version)? ResolveVersionVerbose(bool cpmEnabled, PackageVersionLookup? lookup = null)
        => Resolve(cpmEnabled, lookup ?? new());

    private (Node Node, string Version)? Resolve(bool cpmEnabled, PackageVersionLookup lookup) => cpmEnabled switch
    {
        true when VersionOverride is { Length: > 0 } fixedVersion
            => (this, fixedVersion),

        true when lookup.Reference(Project, IncludeOrUpdate) is { VersionOverride.Length: > 0 } versionOverride
            => (versionOverride, versionOverride.VersionOverride),

        true when lookup.Version(Project, IncludeOrUpdate) is { Version.Length: > 0 } packageVersion
            => (packageVersion, packageVersion.Version),

        _ when Version?.Length > 0
            => (this, Version),

        _ => null,
    };
}
