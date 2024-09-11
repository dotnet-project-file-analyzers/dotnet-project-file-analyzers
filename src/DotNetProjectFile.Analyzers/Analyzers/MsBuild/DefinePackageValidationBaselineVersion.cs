namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefinePackageValidationBaselineVersion() : MsBuildProjectFileAnalyzer(Rule.DefinePackageValidationBaselineVersion)
{
    protected override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (!context.Project.PackageValidationEnabled())
        {
            return;
        }

        if (context.Project
            .ImportsAndSelf()
            .SelectMany(p => p.PropertyGroups)
            .SelectMany(g => g.PackageValidationBaselineVersion).None())
        {
            context.ReportDiagnostic(Description, context.Project);
        }
    }
}
