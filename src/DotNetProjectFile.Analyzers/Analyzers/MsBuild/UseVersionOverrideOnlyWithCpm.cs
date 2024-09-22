namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseVersionOverrideOnlyWithCpm()
    : MsBuildProjectFileAnalyzer(Rule.UseVersionOverrideOnlyWithCpm)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.File.ManagePackageVersionsCentrally() is not true)
        {
            foreach (var reference in context.File.ItemGroups
                .SelectMany(g => g.PackageReferences)
                .Where(r => r.VersionOverride is { Length: > 0 }))
            {
                context.ReportDiagnostic(Descriptor, reference);
            }
        }
    }
}
