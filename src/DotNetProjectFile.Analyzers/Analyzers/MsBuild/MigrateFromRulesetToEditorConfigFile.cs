namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class MigrateFromRulesetToEditorConfigFile()
    : MsBuildProjectFileAnalyzer(Rule.MigrateFromRulesetToEditorConfigFile)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var ruleset in context.Project.PropertyGroups.SelectMany(g => g.CodeAnalysisRuleSet))
        {
            context.ReportDiagnostic(Descriptor, ruleset, ruleset.Value);
        }
    }
}
