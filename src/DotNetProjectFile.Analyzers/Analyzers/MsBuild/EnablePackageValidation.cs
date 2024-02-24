namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class EnablePackageValidation() : MsBuildProjectFileAnalyzer(Rule.EnablePackageValidation)
{
    protected override bool ApplyToProps => false;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (!context.Project.PackageValidationEnabled() && context.Project.IsPackable())
        {
            context.ReportDiagnostic(Descriptor, context.Project);
        }
    }
}
