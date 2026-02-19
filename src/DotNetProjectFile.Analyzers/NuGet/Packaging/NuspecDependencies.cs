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
}
