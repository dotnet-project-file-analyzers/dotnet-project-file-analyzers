using System.Xml.Serialization;

namespace DotNetProjectFile.NuGet.Packaging;

public sealed record NuspecDependencies
{
    [XmlElement("group")]
    public Group[]? Groups { get; init; } = [];

    [XmlElement("dependency")]
    public NuspecDependency[]? Dependencies { get; init; } = [];

    /// <summary>Both <see cref="Groups"/> and <see cref="Dependencies"/>.</summary>
    public IEnumerable<NuspecDependency> All =>
    [
        .. Groups?.SelectMany(g => g.Dependencies ?? []) ?? [],
        .. Dependencies ?? [],
    ];

    /// <summary>
    /// Gets the dependencies that apply to (one of) the <paramref name="targetFrameworks"/>:
    /// per target framework the nearest compatible group, as NuGet resolves them.
    /// </summary>
    /// <remarks>
    /// Falls back to <see cref="All"/> when no target framework is known or
    /// can be parsed, so that no dependency is missed.
    /// </remarks>
    [Pure]
    public IEnumerable<NuspecDependency> For(IReadOnlyCollection<string> targetFrameworks)
    {
        if (Groups is not { Length: > 0 } groups)
        {
            return Dependencies ?? [];
        }

        var projects = targetFrameworks.Select(Frameworks.Parse).ToArray();

        if (projects.Length == 0 || projects.Any(p => p is null))
        {
            return All;
        }

        var candidates = groups.Select(g => Frameworks.Parse(g.TargetFramework)).ToArray();
        var fallback = Array.FindIndex(groups, g => g.TargetFramework is not { Length: > 0 });
        var selected = new HashSet<int>();

        foreach (var project in projects)
        {
            var nearest = Frameworks.Nearest(project!.Value, candidates);
            selected.Add(nearest >= 0 ? nearest : fallback);
        }

        return
        [
            .. selected.Where(i => i >= 0).SelectMany(i => groups[i].Dependencies ?? []),
            .. Dependencies ?? [],
        ];
    }
}
