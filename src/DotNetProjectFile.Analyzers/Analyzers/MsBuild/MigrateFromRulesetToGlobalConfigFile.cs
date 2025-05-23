namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.MigrateFromRulesetToGlobalConfigFile"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class MigrateFromRulesetToGlobalConfigFile()
    : MsBuildProjectFileAnalyzer(Rule.MigrateFromRulesetToGlobalConfigFile)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var ruleset in context.File.PropertyGroups.Children<CodeAnalysisRuleSet>())
        {
            context.ReportDiagnostic(Descriptor, ruleset, ruleset.Value);
        }
    }
}
