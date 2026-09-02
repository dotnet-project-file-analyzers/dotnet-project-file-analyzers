namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>
/// Implements
/// <see cref="Rule.RemoveConfigurationNotConfigurableRule"/>
/// <see cref="Rule.RemoveDroppedRuleConfiguration"/>
/// .</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class RemoveIneffectiveRuleConfiguration() : MsBuildProjectFileAnalyzer(
    Rule.RemoveConfigurationNotConfigurableRule,
    Rule.RemoveDroppedRuleConfiguration)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var node in context.File.DescendantsAndSelf().OfType<WarnBase>())
        {
            foreach (var id in node.RuleIds.Where(RoslynRules.NotConfigurables.Contains))
            {
                if (RoslynRules.NotConfigurables.Contains(id))
                {
                    context.ReportDiagnostic(Rule.RemoveConfigurationNotConfigurableRule, node, id);
                }
                else if (RoslynRules.Dropped.Contains(id))
                {
                    context.ReportDiagnostic(Rule.RemoveDroppedRuleConfiguration, node, id);
                }
            }
        }
    }
}
