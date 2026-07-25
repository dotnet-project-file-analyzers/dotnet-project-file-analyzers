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

    public abstract (Node Node, string Version)? ResolveVersionVerbose(bool cpmEnabled);

    /// <summary>Resolves the version taking CPM into account.</summary>
    public string? ResolveVersion(bool cpmEnabled)
        => ResolveVersionVerbose(cpmEnabled)?.Version;

    public Package? ResolvePackage(bool cpmEnabled)
        => PackageCache.GetPackage(IncludeOrUpdate, ResolveVersion(cpmEnabled));

    public HashSet<Package> ResolveCachedPackageDependencyTree(bool cpmEnabled)
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

        if (!Enqueue(ResolvePackage(cpmEnabled)))
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
