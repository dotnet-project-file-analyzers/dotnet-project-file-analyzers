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

    public (Node Node, string Version)? ResolveVersionVerbose()
    {
        if (Project.ManagePackageVersionsCentrally() is true)
        {
            if (VersionOverride is { Length: > 0 })
            {
                return (this, VersionOverride);
            }

            var versionNode = Project
                .WalkBackward()
                .OfType<PackageVersion>()
                .FirstOrDefault(v => v.Include == IncludeOrUpdate);

            if (versionNode is { } && versionNode.Version is { Length: > 0 } version)
            {
                return (versionNode, version);
            }
        }
        else if (Version is { Length: > 0 })
        {
            return (this, Version);
        }

        return null;
    }

    /// <summary>Resolves the version taking CPM into account.</summary>
    public string? ResolveVersion()
        => ResolveVersionVerbose()?.Version;
}
