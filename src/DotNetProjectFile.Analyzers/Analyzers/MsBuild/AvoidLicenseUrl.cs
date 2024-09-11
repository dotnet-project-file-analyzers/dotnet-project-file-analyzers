namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class AvoidLicenseUrl() : MsBuildProjectFileAnalyzer(Rule.AvoidLicenseUrl)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var lincensUrl in context.Project.PropertyGroups.SelectMany(g => g.PackageLicenseUrl))
        {
            context.ReportDiagnostic(Description, lincensUrl);
        }
    }
}
