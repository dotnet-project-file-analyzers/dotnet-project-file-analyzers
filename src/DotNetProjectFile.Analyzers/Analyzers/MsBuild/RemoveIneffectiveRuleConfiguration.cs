namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.RemoveIneffectiveRuleConfiguration"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class RemoveIneffectiveRuleConfiguration() : MsBuildProjectFileAnalyzer(Rule.RemoveIneffectiveRuleConfiguration)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var node in context.File.DescendantsAndSelf().OfType<WarnBase>())
            foreach (var id in node.RuleIds.Where(RoslynRules.NotConfigurables.Contains))
                context.ReportDiagnostic(Descriptor, node, id);
    }
}
