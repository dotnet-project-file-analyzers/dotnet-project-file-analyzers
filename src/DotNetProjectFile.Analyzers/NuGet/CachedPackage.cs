namespace DotNetProjectFile.NuGet;

public sealed record CachedPackage
{
    public required string Name { get; init; }

    public required string Version { get; init; }

    public required bool HasAnalyzerDll { get; init; }

    public required bool HasRuntimeDll { get; init; }

    public required LicenseExpression License { get; init; }
}
