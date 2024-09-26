namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseAnalyzers() : MsBuildProjectFileAnalyzer(Rule.UseDotNetProjectFileAnalyzers)
{
    protected override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.File
            .Walk()
            .OfType<PackageReference>()
            .None(r => r.Include.Contains("DotNetProjectFile.Analyzers", StringComparison.OrdinalIgnoreCase)))
        {
            context.ReportDiagnostic(Descriptor, context.File);
        }
    }
}
