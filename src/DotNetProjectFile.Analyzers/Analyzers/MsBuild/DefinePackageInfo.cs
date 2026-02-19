namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefinePackageInfo() : MsBuildProjectFileAnalyzer(
    Rule.DefineVersion,
    Rule.DefineDescription,
    Rule.DefineAuthors,
    Rule.DefineTags,
    Rule.DefineRepositoryUrl,
    Rule.DefineUrl,
    Rule.DefineCopyright,
    Rule.DefineReleaseNotes,
    Rule.DefineReadmeFile,
    Rule.DefineLicense,
    Rule.DefineIcon,
    Rule.DefineIconUrl,
    Rule.DefinePackageId,
    Rule.DefineProductName,
    Rule.DefinePackageRequireLicenseAcceptance)
{
    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (!context.File.IsPackable() || context.File.IsTestProject()) return;

        var available = context.File.Walk().Select(n => n.GetType()).ToImmutableHashSet();

        if (!HasAlternativePackageVersioning(context))
        {
            Analyze(context, available, Rule.DefineVersion, typeof(DotNetProjectFile.MsBuild.Version), typeof(VersionPrefix));
        }
        Analyze(context, available, Rule.DefineDescription, typeof(Description), typeof(PackageDescription));
        Analyze(context, available, Rule.DefineAuthors, typeof(Authors));
        Analyze(context, available, Rule.DefineTags, typeof(PackageTags));

        if (!HasSourceLinkEnabled(context))
        {
            Analyze(context, available, Rule.DefineRepositoryUrl, typeof(RepositoryUrl));
        }
        Analyze(context, available, Rule.DefineUrl, typeof(PackageProjectUrl));
        Analyze(context, available, Rule.DefineCopyright, typeof(Copyright));
        Analyze(context, available, Rule.DefineReleaseNotes, typeof(PackageReleaseNotes));
        Analyze(context, available, Rule.DefineReadmeFile, typeof(PackageReadmeFile));
        Analyze(context, available, Rule.DefineIcon, typeof(PackageIcon));
        Analyze(context, available, Rule.DefineIconUrl, typeof(PackageIconUrl));
        Analyze(context, available, Rule.DefinePackageId, typeof(PackageId));
        Analyze(context, available, Rule.DefineLicense, typeof(PackageLicenseFile), typeof(PackageLicenseExpression));
        Analyze(context, available, Rule.DefineProductName, typeof(ProductName));
        Analyze(context, available, Rule.DefinePackageRequireLicenseAcceptance, typeof(PackageRequireLicenseAcceptance));
    }

    private static void Analyze(
        ProjectFileAnalysisContext context,
        ImmutableHashSet<Type> available,
        DiagnosticDescriptor descriptor,
        params Type[] required)
    {
        if (!required.Exists(available.Contains))
        {
            context.ReportDiagnostic(descriptor, context.File);
        }
    }

    private static bool HasAlternativePackageVersioning(ProjectFileAnalysisContext context) => context
        .File.Walk()
        .OfType<PackageReference>()
        .Any(HasAlternativePackageVersioning);

    private static bool HasSourceLinkEnabled(ProjectFileAnalysisContext context)
        => context.Options.GetSdkVersion() >= SdkVersion.NET8;

    private static bool HasAlternativePackageVersioning(PackageReference reference)
        => reference.IncludeOrUpdate
        is "MinVer"
        or "NuGet.Versioning";
}
