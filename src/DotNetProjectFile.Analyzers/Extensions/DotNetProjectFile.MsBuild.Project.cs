namespace DotNetProjectFile.MsBuild;

/// <summary>Provides extension methods for the <see cref="MsBuildProject"/> class.</summary>
internal static class DotNetProjectFileAnalyzerProjectExtensions
{
    public static bool IsPackable(this MsBuildProject project)
        => project.Property<bool?, IsPackable>(g => g.IsPackable, MsBuildDefaults.IsPackable).GetValueOrDefault();

    public static bool PackageValidationEnabled(this MsBuildProject project)
        => project.Property<bool?, EnablePackageValidation>(g => g.EnablePackageValidation, MsBuildDefaults.EnablePackageValidation).GetValueOrDefault();
}
