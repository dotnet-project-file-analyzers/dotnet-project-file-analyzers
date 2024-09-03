namespace DotNetProjectFile.MsBuild;

/// <summary>Provides extension methods for the <see cref="MsBuildProject"/> class.</summary>
internal static class DotNetProjectFileAnalyzerProjectExtensions
{
    public static bool IsPackable(this MsBuildProject project)
        => project.Property(g => g.IsPackable, MsBuildDefaults.IsPackable).GetValueOrDefault();

    public static bool IsTestProject(this MsBuildProject project)
        => project.Property(g => g.IsTestProject, MsBuildDefaults.IsTestProject).GetValueOrDefault();

    public static bool NETAnalyzersEnabled(this MsBuildProject project)
        => project.Property(g => g.EnableNETAnalyzers, MsBuildDefaults.EnableNETAnalyzers).GetValueOrDefault();

    public static bool PackageValidationEnabled(this MsBuildProject project)
        => project.Property(g => g.EnablePackageValidation, MsBuildDefaults.EnablePackageValidation).GetValueOrDefault();

    public static bool IsDevelopmentDependency(this MsBuildProject project)
        => project.Property(g => g.DevelopmentDependency, MsBuildDefaults.DevelopmentDependency).GetValueOrDefault();

    public static bool? ManagePackageVersionsCentrally(this MsBuildProject project)
       => project.Property(g => g.ManagePackageVersionsCentrally, MsBuildDefaults.ManagePackageVersionsCentrally);
}
