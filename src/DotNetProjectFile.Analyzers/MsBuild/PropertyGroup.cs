namespace DotNetProjectFile.MsBuild;

public sealed class PropertyGroup : Node
{
    /// <summary>Initializes a new instance of the <see cref="PropertyGroup"/> class.</summary>
    public PropertyGroup(XElement element, Node parent, Project project) : base(element, parent, project)
    {
        TargetFramework = Children<TargetFramework>();
        TargetFrameworks = Children<TargetFrameworks>();
        ImplicitUsings = Children<ImplicitUsings>();
        NuGetAudit = Children<NuGetAudit>();
        OutputType = Children<OutputType>();

        IsPackable = Children<IsPackable>();
        Version = Children<Version>();
        Description = Children<Description>();
        Authors = Children<Authors>();
        PackageTags = Children<PackageTags>();
        RepositoryUrl = Children<RepositoryUrl>();
        PackageProjectUrl = Children<PackageProjectUrl>();
        Copyright = Children<Copyright>();
        PackageId = Children<PackageId>();
        PackageReleaseNotes = Children<PackageReleaseNotes>();
        PackageReadmeFile = Children<PackageReadmeFile>();
        PackageLicenseExpression = Children<PackageLicenseExpression>();
        PackageLicenseFile = Children<PackageLicenseFile>();
        PackageLicenseUrl = Children<PackageLicenseUrl>();
        PackageIcon = Children<PackageIcon>();
        PackageIconUrl = Children<PackageIconUrl>();
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
}
