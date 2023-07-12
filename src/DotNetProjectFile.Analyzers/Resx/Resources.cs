using DotNetProjectFile.Caching;
using DotNetProjectFile.IO;
using System.IO;

namespace DotNetProjectFile.Resx;

internal sealed class Resources : IReadOnlyCollection<Resource>
{
    private readonly Dictionary<FileInfo, Resource> items = new(FileSystemEqualityComparer.File);

    public int Count => items.Count;

    public IEnumerator<Resource> GetEnumerator() => items.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public static Resources Resolve(Compilation compilation, IEnumerable<AdditionalText> additionalFiles)
        => Cache.Get(compilation, () => New(additionalFiles));

    private static Resources New(IEnumerable<AdditionalText> additionalFiles)
    {
        var resources = new Resources();

        foreach (var additional in additionalFiles
            .Where(a => string.Equals(Path.GetExtension(a.Path), ".resx", StringComparison.OrdinalIgnoreCase)))
        {
            var resource = Resource.Load(additional);
            resources.items[resource.Path] = resource;
        }

        return resources;
    }

    private static readonly CompilationCache<Resources> Cache = new();
}
