namespace DotNetProjectFile.MsBuild;

public static class MsBuildDefaults
{
    public static readonly bool DevelopmentDependency = false;
    public static readonly bool EnableNETAnalyzers = false;
    public static readonly bool EnablePackageValidation = false;
    public static readonly bool IsPackable = true;
    public static readonly bool IsPublishable = true;
    public static readonly bool IsTestProject = false;
    public static readonly bool? ManagePackageVersionsCentrally = null;
    public static readonly bool? NuGetAudit = true;
    public static readonly bool RestorePackagesWithLockFile = false;
    public static readonly bool RestoreLockedMode = false;
    public static readonly bool ContinuousIntegrationBuild = false;
}
