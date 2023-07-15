namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class AvoidLicenseUrl : MsBuildProjectFileAnalyzer
{
    public AvoidLicenseUrl() : base(Rule.AvoidLicenseUrl) { }

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.Project.IsProject)
        {
            var nodes = context.Project
                .ImportsAndSelf()
                .SelectMany(p => p.PropertyGroups)
                .SelectMany(g => g.PackageLicenseUrl);

            if (nodes.Any())
            {
                context.ReportDiagnostic(Descriptor, context.Project);
            }
        }
    }
}
