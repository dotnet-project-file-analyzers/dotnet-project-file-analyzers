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

        /// <summary>Indicates that the .NET analyzers are enabled.</summary>
        public bool EnableNETAnalyzers
            => context.Props.EnableNETAnalyzers
            ?? context.File.Property<EnableNETAnalyzers>()?.Value
            ?? MsBuildDefaults.EnableNETAnalyzers;
    }
}
