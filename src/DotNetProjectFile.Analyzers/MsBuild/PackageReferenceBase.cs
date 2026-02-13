using DotNetProjectFile.NuGet;

namespace DotNetProjectFile.MsBuild;

public abstract class PackageReferenceBase(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project)
{
    public string? Include => Attribute();

    public string? Update => Attribute();

    public string? Version => Attribute();

    public string IncludeOrUpdate => Include ?? Update ?? string.Empty;

    public virtual PackageVersionInfo Info => new(IncludeOrUpdate, Version);

    public (Node Node, string Version)? ResolveVersionVerbose()
    {
        if (Project.ManagePackageVersionsCentrally() is true)
        {
            if (this is PackageReference { VersionOverride: { Length: > 0 } selfVersionOverride })
            {
                return (this, selfVersionOverride);
            }

            var versionOverrideNode = Project
                .WalkBackward()
                .OfType<PackageReference>()
                .FirstOrDefault(p => p.IncludeOrUpdate == IncludeOrUpdate);

            if (versionOverrideNode?.VersionOverride is { Length: > 0 } versionOverride)
            {
                return (versionOverrideNode, versionOverride);
            }

            var versionNode = Project
                .WalkBackward()
                .OfType<PackageVersion>()
                .FirstOrDefault(v => v.Include == IncludeOrUpdate);

            if (versionNode?.Version is { Length: > 0 } version)
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

    public Package? ResolvePackage()
        => PackageCache.GetPackage(IncludeOrUpdate, ResolveVersion());

    public HashSet<Package> ResolveCachedPackageDependencyTree()
    {
        var result = new HashSet<Package>();
        var queue = new Queue<Package>();

        bool Enqueue(Package? pkg)
        {
            if (pkg is { } && result.Add(pkg))
            {
                queue.Enqueue(pkg);
                return true;
            }

            return false;
        }

        if (!Enqueue(ResolvePackage()))
        {
            return result;
        }

        while (queue.Count > 0)
        {
            var cur = queue.Dequeue();

            foreach (var dep in cur.NuSpec?.Metadata?.Dependencies?.All ?? [])
            {
                Enqueue(PackageCache.GetPackage(dep.Id, dep.Version));
            }
        }

        return result;
    }
}
