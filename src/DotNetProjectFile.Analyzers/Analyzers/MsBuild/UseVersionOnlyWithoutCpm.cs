namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseVersionOnlyWithoutCpm()
    : MsBuildProjectFileAnalyzer(Rule.UseVersionOnlyWithoutCpm)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.Project.ManagePackageVersionsCentrally() is true)
        {
            foreach (var reference in context.Project.ItemGroups
                .SelectMany(g => g.PackageReferences)
                .Where(r => r.Version is { Length: > 0 }))
            {
                context.ReportDiagnostic(Descriptor, reference);
            }
        }
    }
}
