using DotNetProjectFile.NuGet.Packaging;

namespace DotNetProjectFile.NuGet;

public sealed record CachedPackage
{
    public required string Name { get; init; }

    public required string Version { get; init; }

    public required bool HasAnalyzerDll { get; init; }

    public required bool HasRuntimeDll { get; init; }

    public bool? IsDevelopmentDependency { get; init; }
    
    public string? License { get; init; }
}
