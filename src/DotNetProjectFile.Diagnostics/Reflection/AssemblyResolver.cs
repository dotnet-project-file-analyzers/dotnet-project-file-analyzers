using System.Reflection;

namespace DotNetProjectFile.Diagnostics.Reflection;

internal sealed class AssemblyResolver : IDisposable
{
    private readonly IReadOnlyCollection<Assembly> Assemblies;

    public AssemblyResolver(IReadOnlyCollection<Assembly> assemblies)
    {
        Assemblies = assemblies;
        AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
    }

    [Pure]
    private Assembly? ResolveAssembly(object? sender, ResolveEventArgs args)
    {
        if (Assemblies.FirstOrDefault(x => x.FullName == args.Name) is { } match)
        {
            return match;
        }

        var requestedName = new AssemblyName(args.Name).Name;
        return Assemblies.FirstOrDefault(x => x.GetName().Name == requestedName);
    }

    public void Dispose()
        => AppDomain.CurrentDomain.AssemblyResolve -= ResolveAssembly;
}
