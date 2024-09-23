using ProjectReference = DotNetProjectFile.MsBuild.ProjectReference;

namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class OrderProjectReferencesAlphabetically() : MsBuildProjectFileAnalyzer(Rule.OrderProjectReferencesAlphabetically)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var references in context.File.ItemGroups.Select(g => g.ProjectReferences))
        {
            AnalyzeGroup(context, references);
        }
    }

    private void AnalyzeGroup(ProjectFileAnalysisContext context, Nodes<ProjectReference> references)
        => references.CheckAlphabeticalOrder(r => r.Include, (expected, found) =>
        {
            context.ReportDiagnostic(Descriptor, expected, expected.Include, found.Include);
        });
}
