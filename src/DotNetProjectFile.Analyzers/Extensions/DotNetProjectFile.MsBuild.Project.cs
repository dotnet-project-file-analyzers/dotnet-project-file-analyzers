namespace DotNetProjectFile.MsBuild;

/// <summary>Provides extension methods for the <see cref="MsBuildProject"/> class.</summary>
internal static class DotNetProjectFileAnalyzerProjectExtensions
{
    public static bool IsPackable(this MsBuildProject project)
         => project.Property<IsPackable>()?.Value ?? MsBuildDefaults.IsPackable;

    public static bool IsPublishable(this MsBuildProject project)
        => project.Property<IsPublishable>()?.Value ?? MsBuildDefaults.IsPublishable;

    public static bool IsTestProject(this MsBuildProject project)
        => project.Property<IsTestProject>()?.Value ?? MsBuildDefaults.IsTestProject;

    public static bool NETAnalyzersEnabled(this MsBuildProject project)
        => project.Property<EnableNETAnalyzers>()?.Value ?? MsBuildDefaults.EnableNETAnalyzers;

    public static bool? NuGetAuditEnabled(this MsBuildProject project)
        => project.Property<NuGetAudit>()?.Value ?? MsBuildDefaults.NuGetAudit;

    public static bool PackageValidationEnabled(this MsBuildProject project)
        => project.Property<EnablePackageValidation>()?.Value ?? MsBuildDefaults.EnablePackageValidation;

    public static bool IsDevelopmentDependency(this MsBuildProject project)
        => project.Property<DevelopmentDependency>()?.Value ?? MsBuildDefaults.DevelopmentDependency;

    public static bool? ManagePackageVersionsCentrally(this MsBuildProject project)
       => project.Property<ManagePackageVersionsCentrally>()?.Value ?? MsBuildDefaults.ManagePackageVersionsCentrally;

    private static TNode? Property<TNode>(this MsBuildProject project) where TNode : Node
        => project.Walk().OfType<TNode>().LastOrDefault(n => n.Parent is PropertyGroup);
}
