using DotNetProjectFile.Licensing;
using DotNetProjectFile.NuGet.Packaging;

namespace DotNetProjectFile.NuGet;

/// <summary>Contains information about a NuGet package.</summary>
public sealed record Package
{
    private readonly Lazy<bool> hasAnalyzerDll;
    private readonly Lazy<bool> hasRuntimeDll;

    /// <summary>Initializes a new instance of the <see cref="Package"/> class.</summary>
    /// <remarks>
    /// The DLL checks walk the package directory, which is expensive, and
    /// not needed by all analyzers. Hence they are only resolved on demand.
    /// </remarks>
    public Package()
    {
        hasAnalyzerDll = new(() => HasDllFiles("analyzers"));
        hasRuntimeDll = new(() => HasDllFiles("lib") || HasDllFiles("runtimes") || HasNativeFiles());
    }

    public required string Name { get; init; }

    public required string Version { get; init; }

    public required IODirectory Directory { get; init; }

    public bool HasAnalyzerDll => hasAnalyzerDll.Value;

    public bool HasRuntimeDll => hasRuntimeDll.Value;

    public required bool HasDependencies { get; init; }

    public required bool? IsDevelopmentDependency { get; init; }

    public required string? LicenseExpression { get; init; }

    public required string? LicenseFile { get; init; }

    public required string? LicenseUrl { get; init; }

    public required LicenseExpression License { get; init; }

    public required NuSpecFile? NuSpec { get; init; }

    public bool IsAnalyzerOnly => HasAnalyzerDll && !HasRuntimeDll;

    /// <remarks>
    /// See: https://learn.microsoft.com/nuget/create-packages/native-files-in-net-packages.
    /// </remarks>
    private bool HasNativeFiles()
        => Directory.SubDirectory("runtimes") is { Exists: true } dir
        && dir.Files("./**/native/*.so")?.Any() is true;

    private bool HasDllFiles(string subDir)
        => Directory.SubDirectory(subDir) is { Exists: true } dir
        && dir.Files("./**/*.dll")?.Any() is true;
}
