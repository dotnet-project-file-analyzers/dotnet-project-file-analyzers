using DotNetProjectFile.IO;
using System.Globalization;
using System.IO;

namespace DotNetProjectFile.Resx;

public sealed class Resources : IReadOnlyCollection<Resource>
{
    private readonly Dictionary<FileInfo, Resource> items = new(FileSystemEqualityComparer.File);

    public int Count => items.Count;

    public IEnumerator<Resource> GetEnumerator() => items.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public static Resources Resolve(IEnumerable<AdditionalText> additionalFiles)
    {
        var resources = new Resources();

        foreach (var additional in additionalFiles
            .Where(a => string.Equals(Path.GetExtension(a.Path), ".resx", StringComparison.OrdinalIgnoreCase)))
        {
            var resource = Resource.Load(additional, resources);
            resources.items[resource.Path] = resource;
        }

        return resources;
    }

    internal IReadOnlyCollection<Resource> Parents(Resource resource)
    {
        if (resource.ForInvariantCulture)
        {
            return Array.Empty<Resource>();
        }
        else
        {
            var parents = resource.Culture.Ancestors()
                .Select(resource.Path.Satellite)
                .Select(file => items.TryGetValue(file, out var parent) ? parent : null)
                .Where(r => r is { })
                .Cast<Resource>()
                .ToArray();

            return parents;
        }
    }
}
