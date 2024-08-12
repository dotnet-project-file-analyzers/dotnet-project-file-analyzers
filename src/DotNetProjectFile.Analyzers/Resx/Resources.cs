using System.Globalization;
using System.IO;

namespace DotNetProjectFile.Resx;

public sealed class Resources : IReadOnlyCollection<Resource>
{
    private readonly Dictionary<IOFile, Resource> items = [];

    public int Count => items.Count;

    public IEnumerator<Resource> GetEnumerator() => items.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    internal IReadOnlyCollection<Resource> Parents(Resource resource)
       => resource.ForInvariantCulture
       ? []
       : (IReadOnlyCollection<Resource>)resource.Culture.Ancestors()
           .Select(resource.Path.Satellite)
           .Select(file => items.TryGetValue(file, out var parent) ? parent : null)
           .OfType<Resource>()
           .ToArray();

    public static Resources Resolve(Compilation compilation, IEnumerable<AdditionalText> additionalFiles)
        => Cache.Get(compilation, () => New(additionalFiles));

    private static Resources New(IEnumerable<AdditionalText> additionalFiles)
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

    private static readonly CompilationCache<Resources> Cache = new();
}
