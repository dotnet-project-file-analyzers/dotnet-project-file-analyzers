namespace DotNetProjectFile.MsBuild;

public sealed class PropertyGroup : Node
{
    /// <summary>Initializes a new instance of the <see cref="PropertyGroup"/> class.</summary>
    public PropertyGroup(XElement element, Node parent, Project project) : base(element, parent, project)
    {
        TargetFramework = Children.Typed<TargetFramework>();
        TargetFrameworks = Children.Typed<TargetFrameworks>();
        ImplicitUsings = Children.Typed<ImplicitUsings>();
        NuGetAudit = Children.Typed<NuGetAudit>();
        OutputType = Children.Typed<OutputType>();

        IsPackable = Children.Typed<IsPackable>();
        IsPublishable = Children.Typed<IsPublishable>();
        Version = Children.Typed<Version>();
        Description = Children.Typed<Description>();
        Authors = Children.Typed<Authors>();
        PackageTags = Children.Typed<PackageTags>();
        RepositoryUrl = Children.Typed<RepositoryUrl>();
        PackageProjectUrl = Children.Typed<PackageProjectUrl>();
        Copyright = Children.Typed<Copyright>();
        PackageDescription = Children.Typed<PackageDescription>();
        PackageId = Children.Typed<PackageId>();
        PackageLicenseExpression = Children.Typed<PackageLicenseExpression>();
        PackageLicenseFile = Children.Typed<PackageLicenseFile>();
        PackageLicenseUrl = Children.Typed<PackageLicenseUrl>();
        PackageIcon = Children.Typed<PackageIcon>();
        PackageIconUrl = Children.Typed<PackageIconUrl>();
        PackageReleaseNotes = Children.Typed<PackageReleaseNotes>();
        PackageReadmeFile = Children.Typed<PackageReadmeFile>();
        GeneratePackageOnBuild = Children.Typed<GeneratePackageOnBuild>();
    }

    public Nodes<TargetFramework> TargetFramework { get; }

    public Nodes<TargetFrameworks> TargetFrameworks { get; }

    public Nodes<ImplicitUsings> ImplicitUsings { get; }

    public Nodes<NuGetAudit> NuGetAudit { get; }

    public Nodes<OutputType> OutputType { get; }

    public Nodes<IsPackable> IsPackable { get; }

    public Nodes<Version> Version { get; }

    public Nodes<Description> Description { get; }

    public Nodes<Authors> Authors { get; }

    public Nodes<PackageTags> PackageTags { get; }

    public Nodes<RepositoryUrl> RepositoryUrl { get; }

    public Nodes<PackageId> PackageId { get; }

    public Nodes<PackageProjectUrl> PackageProjectUrl { get; }

    public Nodes<Copyright> Copyright { get; }

    public Nodes<PackageReleaseNotes> PackageReleaseNotes { get; }

    public Nodes<PackageDescription> PackageDescription { get; }

    public Nodes<PackageLicenseExpression> PackageLicenseExpression { get; }

    public Nodes<PackageLicenseFile> PackageLicenseFile { get; }

    public Nodes<PackageLicenseUrl> PackageLicenseUrl { get; }

    public Nodes<PackageIcon> PackageIcon { get; }

    public Nodes<PackageIconUrl> PackageIconUrl { get; }

    public Nodes<PackageReadmeFile> PackageReadmeFile { get; }

    public Nodes<IsPublishable> IsPublishable { get; }

    public Nodes<GeneratePackageOnBuild> GeneratePackageOnBuild { get; }
}
