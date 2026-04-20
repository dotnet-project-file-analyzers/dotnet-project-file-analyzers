namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseVersionOnlyWithoutCpm()
    : MsBuildProjectFileAnalyzer(Rule.UseVersionOnlyWithoutCpm)
{
    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.ManagePackageVersionsCentrally)
        {
            foreach (var reference in context.File.ItemGroups
                .Children<PackageReference>(r => r.Version is { Length: > 0 }))
            {
                context.ReportDiagnostic(Descriptor, reference);
            }
        }
    }
}
