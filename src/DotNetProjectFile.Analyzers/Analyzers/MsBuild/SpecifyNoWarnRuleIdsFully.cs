namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.SpecifyNoWarnRuleIdsFully"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class SpecifyNoWarnRuleIdsFully() : MsBuildProjectFileAnalyzer(Rule.SpecifyNoWarnRuleIdsFully)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => true;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext<MsBuildProject> context)
    {
        foreach (var noWarn in context.File.PropertyGroups.Children<NoWarn>())
        {
            foreach (var ruleId in noWarn.RuleIds.Where(NotFullySpecified))
            {
                context.ReportDiagnostic(Descriptor, noWarn, ruleId);
            }
        }
    }

    private bool NotFullySpecified(string ruleId)
        => int.TryParse(ruleId, out _);
}
