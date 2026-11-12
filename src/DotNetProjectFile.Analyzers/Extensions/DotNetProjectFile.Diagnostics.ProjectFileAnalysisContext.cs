namespace DotNetProjectFile.Diagnostics;

internal static class ProjectFileAnalysisContextExtensions
{
    extension(ProjectFileAnalysisContext context)
    {
        /// <summary>Indicates that the .NET analyzers are enabled.</summary>
        public bool IsDevelopmentDependency
            => context.Props.DevelopmentDependency
            ?? context.File.Property<DevelopmentDependency>()?.Value
            ?? MsBuildDefaults.DevelopmentDependency;

        /// <summary>Indicates that CPM is enabled.</summary>
        public bool ManagePackageVersionsCentrally
            => context.Props.ManagePackageVersionsCentrally
            ?? context.File.Property<ManagePackageVersionsCentrally>()?.Value
            ?? MsBuildDefaults.ManagePackageVersionsCentrally;

        /// <summary>The target framework of the (inner) build, or else the ones defined in the project file.</summary>
        public IReadOnlyCollection<string> TargetFrameworks => context switch
        {
            _ when context.Props.TargetFramework is { Length: > 0 } current => [current],
            _ when context.File.Property<TargetFrameworks>()?.Value is { Count: > 0 } multiple => [.. multiple],
            _ when context.File.Property<TargetFramework>()?.Value is { Length: > 0 } single => [single],
            _ => [],
        };

        /// <summary>Indicates that the .NET analyzers are enabled.</summary>
        public bool EnableNETAnalyzers
            => context.Props.EnableNETAnalyzers
            ?? context.File.Property<EnableNETAnalyzers>()?.Value
            ?? MsBuildDefaults.EnableNETAnalyzers;
    }
}
