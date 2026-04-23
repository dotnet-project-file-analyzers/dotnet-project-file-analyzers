namespace DotNetProjectFile.Diagnostics;

internal static class DotNetProjectFile
{
    extension(ProjectFileAnalysisContext context)
    {
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

        /// <summary>Indicates that restore is run in locked mode.</summary>
        public bool RestoreLockedMode
            => context.Props.RestoreLockedMode
             ?? context.File.Property<RestoreLockedMode>()?.Value
            ?? MsBuildDefaults.RestoreLockedMode;

    }
}
