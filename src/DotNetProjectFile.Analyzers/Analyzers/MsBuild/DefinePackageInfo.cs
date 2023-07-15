namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefinePackageInfo : MsBuildProjectFileAnalyzer
{
    public DefinePackageInfo() : base(
        Rule.DefineVersion,
        Rule.DefineDescription,
        Rule.DefineAuthors,
        Rule.DefineTags,
        Rule.DefineRepositoryUrl,
        Rule.DefineUrl,
        Rule.DefineCopyright,
        Rule.DefineReleaseNotes,
        Rule.DefineReadmeFile,
        Rule.DefineLicenseExpression,
        Rule.DefineLicenseFile,
        Rule.DefineLicenseUrl,
        Rule.DefineIcon,
        Rule.DefineIconUrl) { }

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (!IsLikelyPackableProject(context))
        {
            return;
        }

        Analyze(context, Rule.DefineVersion, g => g.Version);
        Analyze(context, Rule.DefineDescription, g => g.Description);
        Analyze(context, Rule.DefineAuthors, g => g.Authors);
        Analyze(context, Rule.DefineTags, g => g.PackageTags);
        Analyze(context, Rule.DefineRepositoryUrl, g => g.RepositoryUrl);
        Analyze(context, Rule.DefineUrl, g => g.PackageProjectUrl);
        Analyze(context, Rule.DefineCopyright, g => g.Copyright);
        Analyze(context, Rule.DefineReleaseNotes, g => g.PackageReleaseNotes);
        Analyze(context, Rule.DefineReadmeFile, g => g.PackageReadmeFile);
        Analyze(context, Rule.DefineLicenseExpression, g => g.PackageLicenseExpression);
        Analyze(context, Rule.DefineLicenseFile, g => g.PackageLicenseFile);
        Analyze(context, Rule.DefineLicenseUrl, g => g.PackageLicenseUrl);
        Analyze(context, Rule.DefineIcon, g => g.PackageIcon);
        Analyze(context, Rule.DefineIconUrl, g => g.PackageIconUrl);
    }

    private static bool IsLikelyPackableProject(ProjectFileAnalysisContext context)
        => context.Project.IsProject
        && context.Project
            .ImportsAndSelf()
            .SelectMany(p => p.PropertyGroups)
            .SelectMany(g => g.IsPackable)
            .Any(n => n.Value == true);

    private static IEnumerable<T> GetNodes<T>(ProjectFileAnalysisContext context, Func<PropertyGroup, Nodes<T>> getNodes)
        where T : Node
        => context.Project
            .ImportsAndSelf()
            .SelectMany(p => p.PropertyGroups)
            .SelectMany(g => getNodes(g));

    private static void Analyze<T>(ProjectFileAnalysisContext context, DiagnosticDescriptor descriptor, Func<PropertyGroup, Nodes<T>> getNodes)
        where T : Node
    {
        var found = GetNodes<T>(context, getNodes);

        if (found.None())
        {
            context.ReportDiagnostic(descriptor, context.Project);
        }
    }
}
