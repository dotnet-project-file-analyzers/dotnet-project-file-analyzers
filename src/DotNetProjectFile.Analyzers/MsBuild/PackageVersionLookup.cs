namespace DotNetProjectFile.MsBuild;

/// <summary>
/// The first &lt;PackageReference&gt; and &lt;PackageVersion&gt; per package, in
/// <see cref="MsBuildProject.WalkBackward()"/> order. Each project is walked only once.
/// </summary>
/// <remarks>
/// Not thread-safe. Use one per analysis: do not cache it, as imported files can change in between.
/// </remarks>
public sealed class PackageVersionLookup
{
    private readonly Dictionary<MsBuildProject, Entries> Projects = [];

    internal PackageReference? Reference(MsBuildProject project, string name)
        => Get(project).References.TryGetValue(name, out var reference) ? reference : null;

    internal PackageVersion? Version(MsBuildProject project, string name)
        => Get(project).Versions.TryGetValue(name, out var version) ? version : null;

    private Entries Get(MsBuildProject project)
    {
        if (!Projects.TryGetValue(project, out var entries))
        {
            entries = new();

            foreach (var node in project.WalkBackward())
            {
                if (node is PackageReference reference)
                {
                    entries.References.TryAddFirst(reference.IncludeOrUpdate, reference);
                }
                else if (node is PackageVersion { Include: { } include } version)
                {
                    entries.Versions.TryAddFirst(include, version);
                }
            }

            Projects[project] = entries;
        }

        return entries;
    }

    private sealed class Entries
    {
        public readonly Dictionary<string, PackageReference> References = new(StringComparer.Ordinal);

        public readonly Dictionary<string, PackageVersion> Versions = new(StringComparer.Ordinal);
    }
}

file static class Extensions
{
    public static void TryAddFirst<T>(this Dictionary<string, T> dictionary, string key, T value)
    {
        if (!dictionary.ContainsKey(key)) dictionary[key] = value;
    }
}
