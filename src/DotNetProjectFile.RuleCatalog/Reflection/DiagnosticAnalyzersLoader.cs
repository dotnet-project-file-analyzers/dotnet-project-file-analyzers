using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace DotNetProjectFile.RuleCatalog.Reflection;

internal sealed class DiagnosticAnalyzersLoader : IDisposable
{
    private readonly AssemblyLoadContext Context = new("AssemblyLoaderContext", isCollectible: true);

    public DiagnosticAnalyzersLoader() => Context.Resolving += OnResolveDependency;

    public ImmutableArray<DiagnosticAnalyzer> Load(Stream stream)
    {
        try
        {
            var assembly = Context.LoadFromStream(stream);
            return [.. assembly.GetTypes().Select(Analyzers).OfType<DiagnosticAnalyzer>()];
        }
        catch (BadImageFormatException)
        {
            return [];
        }
    }

    private static Assembly? OnResolveDependency(AssemblyLoadContext context, AssemblyName name) => null;

    private static DiagnosticAnalyzer? Analyzers(Type type)
    {
        if (!IsDiagnosticAnalyzer(type)) return null;

        try
        {
            return Activator.CreateInstance(type) as DiagnosticAnalyzer;
        }
        catch
        {
            return null;
        }
    }

    private static bool IsDiagnosticAnalyzer(Type type)
        => !type.IsAbstract
        && type.IsAssignableTo(typeof(DiagnosticAnalyzer))
        && type.GetConstructors().Any(c => c.GetParameters().Length is 0)
        && type.GetCustomAttribute<DiagnosticAnalyzerAttribute>() is { };

    public void Dispose()
    {
        if (!IsDisposed)
        {
            Context.Unload();
            Context.Resolving -= OnResolveDependency;
            IsDisposed = true;
        }
    }

    private bool IsDisposed;
}
