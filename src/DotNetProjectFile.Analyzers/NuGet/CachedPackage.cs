using DotNetProjectFile.NuGet.Packaging;

namespace DotNetProjectFile.NuGet;

public sealed record CachedPackage
{
    public required string Name { get; init; }

    public required string Version { get; init; }

    public required bool HasAnalyzerDll { get; init; }

    public required bool HasRuntimeDll { get; init; }

    public required bool? IsDevelopmentDependency { get; init; }

    public required string? LicenseExpression { get; init; }

    public required string? LicenseFile { get; init; }
    
    public required string? LicenseUrl { get; init; }

    public required LicenseExpression? License { get; init; }
}
