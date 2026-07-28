using DotNetProjectFile.Diagnostics.IO;
using DotNetProjectFile.Diagnostics.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using System.Collections.Immutable;
using System.IO;
using System.Reflection;

namespace DotNetProjectFile.Diagnostics;

/// <summary>NuGet Repository.</summary>
/// <remarks>
/// Subset of NuGet functionality to download analyzer files.
/// </remarks>
public static class NuGetRepository
{
    private static readonly Uri NuGetV3 = new("https://api.nuget.org/v3/index.json");

    /// <summary>Gets all versions that are newer then the current version of the NuGet package.</summary>
    /// <param name="package">
    /// The package to query for.
    /// </param>
    /// <param name="cancellation">
    /// A cancelation token.
    /// </param>
    /// <returns>
    /// The newer versions.
    /// </returns>
    [Pure]
    public static async ValueTask<ImmutableArray<NuGetVersion>> NewVersions(NugetPackage package, CancellationToken cancellation = default)
    {
        var repo = await Repo();
        using var context = new SourceCacheContext();
        NuGetVersion[] all = [.. await repo.GetAllVersionsAsync(package.Id, context, NullLogger.Instance, cancellation)];

        return [.. all.Where(Include).Order()];

        bool Include(NuGetVersion version)
            => !version.IsPrerelease
            && (package.Version is null || version > package.Version);
    }

    /// <summary>Fetches  the diagnostics for the NuGet package.</summary>
    /// <param name="package">
    /// The package to query for.
    /// </param>
    /// <param name="cancellation">
    /// A cancelation token.
    /// </param>
    /// <returns>
    /// The newer versions.
    /// </returns>
    [Pure]
    public static async ValueTask<ImmutableArray<DiagnosticInfo>> Diagnostics(NugetPackage package, CancellationToken cancellation = default)
    {
        var resource = await Repo();
        using var stream = new MemoryStream();
        using var context = new SourceCacheContext();

        await resource.CopyNupkgToStreamAsync(package.Id, package.Version, stream, context, NullLogger.Instance, cancellation);

        using var packageReader = new PackageArchiveReader(stream);

        using var dir = new TemporaryDirectory();

        foreach (var file in packageReader.GetFiles())
        {
            packageReader.ExtractFile(file, Path.Combine(dir.FullName, file), NullLogger.Instance);
        }

        var folders = dir.GetDlls();

        return [.. await FetchDiagnostics(package, folders, cancellation)];
    }

    [Pure]
    private static async ValueTask<ImmutableArray<DiagnosticInfo>> FetchDiagnostics(NugetPackage package, IEnumerable<FileInfo> dlls, CancellationToken cancellation)
    {
        var assemblies = new List<Assembly>();
        var diagnostics = new List<DiagnosticInfo>();

        foreach (var dll in dlls)
        {
            using var stream = new MemoryStream();
            using var reader = dll.OpenRead();

            await reader.CopyToAsync(stream, cancellation);

            try
            {
                assemblies.Add(Assembly.Load(stream.ToArray()));
            }
            catch (BadImageFormatException)
            {
                // Not a .NET dll.
            }
        }

        using (new AssemblyResolver(assemblies))
        {
            foreach (var assembly in assemblies)
            {
                Type[] types;

                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException)
                {
                    continue;
                }

                var analyzers = types.Select(Analyzers).OfType<DiagnosticAnalyzer>();

                foreach (var analyzer in analyzers)
                {
                    var languages = analyzer.GetType().GetCustomAttribute<DiagnosticAnalyzerAttribute>()!.Languages;
                    var obsolete = analyzer
                        .GetType()
                        .GetCustomAttribute<ObsoleteAttribute>()?.Message;

                    foreach (var desc in SupportedDiagnostics(analyzer))
                    {
                        var diagnostic = DiagnosticInfo.New(desc)
                        with
                        {
                            Version = package.Version,
                            First = package.Version,
                            Languages = [.. languages],
                            Obsolete = obsolete,
                        };
                        diagnostics.Add(diagnostic);
                    }
                }
            }
            return [.. diagnostics];
        }

        static bool IsDiagnosticAnalyzer(Type type)
            => !type.IsAbstract
            && type.IsAssignableTo(typeof(DiagnosticAnalyzer))
            && type.GetConstructors().Any(c => c.GetParameters().Length is 0)
            && type.GetCustomAttribute<DiagnosticAnalyzerAttribute>() is { };

        static DiagnosticAnalyzer? Analyzers(Type type)
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

        static IEnumerable<DiagnosticDescriptor> SupportedDiagnostics(DiagnosticAnalyzer analyzers)
        {
            try
            {
                return analyzers.SupportedDiagnostics.OfType<DiagnosticDescriptor>();
            }
            catch
            {
                return [];
            }
        }
    }

    [Pure]
    private static Task<FindPackageByIdResource> Repo()
        => Repository.Factory.GetCoreV3(NuGetV3.AbsoluteUri).GetResourceAsync<FindPackageByIdResource>();
}
