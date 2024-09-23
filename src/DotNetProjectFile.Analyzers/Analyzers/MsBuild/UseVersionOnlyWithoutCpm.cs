namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseVersionOnlyWithoutCpm()
    : MsBuildProjectFileAnalyzer(Rule.UseVersionOnlyWithoutCpm)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.File.ManagePackageVersionsCentrally() is true)
        {
            foreach (var reference in context.File.ItemGroups
                .SelectMany(g => g.PackageReferences)
                .Where(r => r.Version is { Length: > 0 }))
            {
                context.ReportDiagnostic(Descriptor, reference);
            }
        }
    }
}
