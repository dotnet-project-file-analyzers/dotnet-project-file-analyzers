namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseSonarAnalyzers() : MsBuildProjectFileAnalyzer(Rule.UseSonarAnalyzers)
{
    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (Include(context.Compilation.Options.Language) is { } include
            && context.File
                .Walk()
                .OfType<PackageReference>().None(p => p.Include.IsMatch(include)))
        {
            context.ReportDiagnostic(Descriptor, context.File, include);
        }
    }

    private static string? Include(string language) => language switch
    {
        LanguageNames.CSharp => "SonarAnalyzer.CSharp",
        LanguageNames.VisualBasic => "SonarAnalyzer.VisualBasic",
        _ => null,
    };
}
