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
        => Assemblies.FirstOrDefault(x => x.FullName == args.Name);

    public void Dispose()
        => AppDomain.CurrentDomain.AssemblyResolve -= ResolveAssembly;
}
