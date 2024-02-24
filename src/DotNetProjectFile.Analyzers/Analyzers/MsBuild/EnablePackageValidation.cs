namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class EnablePackageValidation() : MsBuildProjectFileAnalyzer(Rule.EnablePackageValidation)
{
    protected override bool ApplyToProps => false;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        var project = context.Project;
        if (project.IsPackable() && !project.IsDevelopmentDependency() && !project.PackageValidationEnabled())
        {
            context.ReportDiagnostic(Descriptor, context.Project);
        }
    }
}
