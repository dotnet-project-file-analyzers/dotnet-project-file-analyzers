namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class StaticAliasUsingNotSupported() : MsBuildProjectFileAnalyzer(Rule.StaticAliasUsingNotSupported)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var directive in context.Project
            .ImportsAndSelf()
            .SelectMany(p => p.ItemGroups)
            .SelectMany(g => g.Usings)
            .Where(u => u.Type == UsingType.StaticAlias))
        {
            context.ReportDiagnostic(Description, directive, directive.Include);
        }
    }
}
