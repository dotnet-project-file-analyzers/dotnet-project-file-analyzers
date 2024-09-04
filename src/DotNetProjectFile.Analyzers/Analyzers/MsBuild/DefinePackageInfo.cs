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
    Rule.DefinePackageId)
{
    protected override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (!context.Project.IsPackable() || context.Project.IsTestProject()) return;

        Analyze(context, Rule.DefineVersion, g => g.Version);
        Analyze(context, Rule.DefineDescription, g => Nodes.Concat(g.Description, g.PackageDescription));
        Analyze(context, Rule.DefineAuthors, g => g.Authors);
        Analyze(context, Rule.DefineTags, g => g.PackageTags);
        Analyze(context, Rule.DefineRepositoryUrl, g => g.RepositoryUrl);
        Analyze(context, Rule.DefineUrl, g => g.PackageProjectUrl);
        Analyze(context, Rule.DefineCopyright, g => g.Copyright);
        Analyze(context, Rule.DefineReleaseNotes, g => g.PackageReleaseNotes);
        Analyze(context, Rule.DefineReadmeFile, g => g.PackageReadmeFile);
        Analyze(context, Rule.DefineIcon, g => g.PackageIcon);
        Analyze(context, Rule.DefineIconUrl, g => g.PackageIconUrl);
        Analyze(context, Rule.DefinePackageId, g => g.PackageId);
        Analyze(context, Rule.DefineLicense, g => Nodes.Concat(g.PackageLicenseFile, g.PackageLicenseExpression));
    }

    private static IEnumerable<Node> GetNodes(ProjectFileAnalysisContext context, Func<PropertyGroup, IEnumerable<Node>> getNodes)
        => context.Project
            .ImportsAndSelf()
            .SelectMany(p => p.PropertyGroups)
            .SelectMany(g => getNodes(g));

    private static void Analyze(ProjectFileAnalysisContext context, DiagnosticDescriptor descriptor, Func<PropertyGroup, IEnumerable<Node>> getNodes)
    {
        var found = GetNodes(context, getNodes);

        if (found.None())
        {
            context.ReportDiagnostic(descriptor, context.Project);
        }
    }
}
