using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace DotNetProjectFile.Diagnostics.Reflection;

internal sealed class AnalyzerLoadContext : AssemblyLoadContext
{
    private readonly Dictionary<string, Assembly> LoadedAssemblies = new(StringComparer.Ordinal);

    public AnalyzerLoadContext() : base(isCollectible: true) { }

    public Assembly LoadAssemblyFromStream(Stream stream)
    {
        var assembly = LoadFromStream(stream);
        if (assembly.FullName is { } fullName)
        {
            LoadedAssemblies[fullName] = assembly;
        }
        return assembly;
    }

    protected override Assembly? Load(AssemblyName assemblyName)
        => assemblyName.FullName is { } fullName && LoadedAssemblies.TryGetValue(fullName, out var match)
            ? match
            : LoadedAssemblies.Values.FirstOrDefault(x => x.GetName().Name == assemblyName.Name);
}
