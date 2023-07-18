namespace DotNetProjectFile.MsBuild;

public sealed class PropertyGroup : Node
{
    /// <summary>Initializes a new instance of the <see cref="PropertyGroup"/> class.</summary>
    public PropertyGroup(XElement element, Node parent, Project project) : base(element, parent, project)
    {
        TargetFramework = Children.OfType<TargetFramework>();
        TargetFrameworks = Children.OfType<TargetFrameworks>();
        ImplicitUsings = Children.OfType<ImplicitUsings>();
        NuGetAudit = Children.OfType<NuGetAudit>();
        OutputType = Children.OfType<OutputType>();

        IsPackable = Children.OfType<IsPackable>();
        Version = Children.OfType<Version>();
        Description = Children.OfType<Description>();
        Authors = Children.OfType<Authors>();
        PackageTags = Children.OfType<PackageTags>();
        RepositoryUrl = Children.OfType<RepositoryUrl>();
        PackageProjectUrl = Children.OfType<PackageProjectUrl>();
        Copyright = Children.OfType<Copyright>();
        PackageId = Children.OfType<PackageId>();
        PackageReleaseNotes = Children.OfType<PackageReleaseNotes>();
        PackageReadmeFile = Children.OfType<PackageReadmeFile>();
        PackageLicenseExpression = Children.OfType<PackageLicenseExpression>();
        PackageLicenseFile = Children.OfType<PackageLicenseFile>();
        PackageLicenseUrl = Children.OfType<PackageLicenseUrl>();
        PackageIcon = Children.OfType<PackageIcon>();
        PackageIconUrl = Children.OfType<PackageIconUrl>();
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

    public Nodes<PackageReadmeFile> PackageReadmeFile { get; }

    public Nodes<PackageLicenseExpression> PackageLicenseExpression { get; }

    public Nodes<PackageLicenseFile> PackageLicenseFile { get; }

    public Nodes<PackageLicenseUrl> PackageLicenseUrl { get; }

    public Nodes<PackageIcon> PackageIcon { get; }

    public Nodes<PackageIconUrl> PackageIconUrl { get; }

    public Nodes<IsPublishable> IsPublishable { get; }
}
