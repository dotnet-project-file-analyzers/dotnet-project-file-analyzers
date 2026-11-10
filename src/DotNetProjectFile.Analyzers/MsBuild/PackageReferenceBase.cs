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

    /// <param name="cpmEnabled">Indicates that CPM is enabled.</param>
    /// <param name="lookup">Pass a shared lookup when resolving multiple references.</param>
    public abstract (Node Node, string Version)? ResolveVersionVerbose(bool cpmEnabled, PackageVersionLookup? lookup = null);

    /// <summary>Resolves the version taking CPM into account.</summary>
    public string? ResolveVersion(bool cpmEnabled, PackageVersionLookup? lookup = null)
        => ResolveVersionVerbose(cpmEnabled, lookup)?.Version;

    public Package? ResolvePackage(bool cpmEnabled, PackageVersionLookup? lookup = null)
        => PackageCache.GetPackage(IncludeOrUpdate, ResolveVersion(cpmEnabled, lookup));

    /// <summary>
    /// Resolves the package and its transitive dependencies that apply to the <paramref name="targetFrameworks"/>.
    /// </summary>
    public IReadOnlyCollection<Package> ResolveCachedPackageDependencyTree(
        bool cpmEnabled,
        IReadOnlyCollection<string> targetFrameworks,
        PackageVersionLookup? lookup = null)
    {
        var result = new List<Package>();
        var resolved = new HashSet<PackageVersionInfo>();
        var queue = new Queue<Package>();

        void Enqueue(Package? pkg)
        {
            // Different versions (ranges) can resolve to the same package: handle it only once.
            if (pkg is { } && resolved.Add(new(pkg.Name, pkg.Version)))
            {
                result.Add(pkg);
                queue.Enqueue(pkg);
            }
        }

        Enqueue(ResolvePackage(cpmEnabled, lookup));

        while (queue.Count > 0)
        {
            foreach (var dep in queue.Dequeue().NuSpec?.Metadata?.Dependencies?.For(targetFrameworks) ?? [])
            {
                Enqueue(PackageCache.GetPackage(dep.Id, dep.Version));
            }
        }

        return result;
    }
}
