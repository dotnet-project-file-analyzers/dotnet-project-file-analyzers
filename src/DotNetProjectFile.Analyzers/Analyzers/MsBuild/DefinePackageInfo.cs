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
    Rule.DefineProductName)
{
    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (!context.File.IsPackable() || context.File.IsTestProject()) return;

        var available = context.File.Walk().Select(n => n.GetType()).ToImmutableHashSet();

        Analyze(context, available, Rule.DefineVersion, typeof(DotNetProjectFile.MsBuild.Version));
        Analyze(context, available, Rule.DefineDescription, typeof(Description), typeof(PackageDescription));
        Analyze(context, available, Rule.DefineAuthors, typeof(Authors));
        Analyze(context, available, Rule.DefineTags, typeof(PackageTags));
        Analyze(context, available, Rule.DefineRepositoryUrl, typeof(RepositoryUrl));
        Analyze(context, available, Rule.DefineUrl, typeof(PackageProjectUrl));
        Analyze(context, available, Rule.DefineCopyright, typeof(Copyright));
        Analyze(context, available, Rule.DefineReleaseNotes, typeof(PackageReleaseNotes));
        Analyze(context, available, Rule.DefineReadmeFile, typeof(PackageReadmeFile));
        Analyze(context, available, Rule.DefineIcon, typeof(PackageIcon));
        Analyze(context, available, Rule.DefineIconUrl, typeof(PackageIconUrl));
        Analyze(context, available, Rule.DefinePackageId, typeof(PackageId));
        Analyze(context, available, Rule.DefineLicense, typeof(PackageLicenseFile), typeof(PackageLicenseExpression));
        Analyze(context, available, Rule.DefineProductName, typeof(ProductName));
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
}
