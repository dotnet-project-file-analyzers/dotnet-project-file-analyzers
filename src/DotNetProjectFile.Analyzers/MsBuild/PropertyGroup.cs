namespace DotNetProjectFile.MsBuild;

public sealed class PropertyGroup(XElement element, Node parent, MsBuildProject project) : Node(element, parent, project)
{
    public Nodes<CodeAnalysisRuleSet> CodeAnalysisRuleSet => new(Children);

    public Nodes<TargetFramework> TargetFramework => new(Children);

    public Nodes<TargetFrameworks> TargetFrameworks => new(Children);

    public Nodes<ImplicitUsings> ImplicitUsings => new(Children);

    public Nodes<NuGetAudit> NuGetAudit => new(Children);

    public Nodes<EnableNETAnalyzers> EnableNETAnalyzers => new(Children);

    public Nodes<OutputType> OutputType => new(Children);

    public Nodes<DevelopmentDependency> DevelopmentDependency => new(Children);

    public Nodes<IsPackable> IsPackable => new(Children);

    public Nodes<Version> Version => new(Children);

    public Nodes<Description> Description => new(Children);

    public Nodes<Authors> Authors => new(Children);

    public Nodes<PackageTags> PackageTags => new(Children);

    public Nodes<RepositoryUrl> RepositoryUrl => new(Children);

    public Nodes<PackageId> PackageId => new(Children);

    public Nodes<PackageProjectUrl> PackageProjectUrl => new(Children);

    public Nodes<Copyright> Copyright => new(Children);

    public Nodes<PackageReleaseNotes> PackageReleaseNotes => new(Children);

    public Nodes<PackageDescription> PackageDescription => new(Children);

    public Nodes<PackageLicenseExpression> PackageLicenseExpression => new(Children);

    public Nodes<PackageLicenseFile> PackageLicenseFile => new(Children);

    public Nodes<PackageLicenseUrl> PackageLicenseUrl => new(Children);

    public Nodes<PackageIcon> PackageIcon => new(Children);

    public Nodes<PackageIconUrl> PackageIconUrl => new(Children);

    public Nodes<PackageReadmeFile> PackageReadmeFile => new(Children);

    public Nodes<IsPublishable> IsPublishable => new(Children);

    public Nodes<IsTestProject> IsTestProject => new(Children);

    public Nodes<GeneratePackageOnBuild> GeneratePackageOnBuild => new(Children);

    public Nodes<GenerateSBOM> GenerateSBOM => new(Children);

    public Nodes<EnablePackageValidation> EnablePackageValidation => new(Children);

    public Nodes<PackageValidationBaselineVersion> PackageValidationBaselineVersion => new(Children);

    public Nodes<ManagePackageVersionsCentrally> ManagePackageVersionsCentrally => new(Children);
}
