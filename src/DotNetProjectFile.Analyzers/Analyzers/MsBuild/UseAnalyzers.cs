namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseAnalyzers() : MsBuildProjectFileAnalyzer(Rule.UseDotNetProjectFileAnalyzers)
{
    protected override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        var packageReferences = context.Project
            .ImportsAndSelf()
            .SelectMany(p => p.ItemGroups)
            .SelectMany(group => group.PackageReferences);

        if (packageReferences.None(r => r.Include.Contains("DotNetProjectFile.Analyzers", StringComparison.OrdinalIgnoreCase)))
        {
            context.ReportDiagnostic(Descriptor, context.Project);
        }
    }
}
