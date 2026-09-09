using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace DotNetProjectFile.RuleCatalog.Reflection;

public sealed class DiagnosticAnalyzersLoader : IDisposable
{
    private readonly AssemblyLoadContext Context = new("AssemblyLoaderContext", isCollectible: true);
    private readonly DirectoryInfo Root;

    public DiagnosticAnalyzersLoader(DirectoryInfo root)
    {
        Root = root;
        Context.Resolving += OnResolveDependency;
    }

    public ImmutableArray<DiagnosticAnalyzer> Load()
    {
        foreach (var location in Root.GetDlls())
        {
            try
            {
                using var stream = location.OpenRead();
                Context.LoadFromStream(stream);
            }
            catch (FileLoadException x) when (x.Message.EndsWith("Assembly with same name is already loaded"))
            {
                // It can occur that an assembly is already as a dependency.
            }
            catch (BadImageFormatException)
            {
                // No .NET dll
            }
        }

        return
        [
            .. Context.Assemblies
                .SelectMany(Types)
                .Where(IsDiagnosticAnalyzer)
                .Select(Analyzers)
                .OfType<DiagnosticAnalyzer>()
        ];
    }

    private static IEnumerable<Type> Types(Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException x)
        {
            return x.Types.OfType<Type>();
        }
        catch (Exception)
        {
            return [];
        }
    }

    private static Assembly? OnResolveDependency(AssemblyLoadContext context, AssemblyName name) => null;

    private static DiagnosticAnalyzer? Analyzers(Type type)
    {
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
