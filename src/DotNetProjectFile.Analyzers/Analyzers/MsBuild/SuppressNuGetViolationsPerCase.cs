namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>
/// Implements <see cref="Rule.SuppressNuGetIssuesPerPackage"/>
/// and <see cref="Rule.SuppressNuGetAuditIssuesViaNuGetAuditSuppress"/>.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class SuppressNuGetViolationsPerCase() : MsBuildProjectFileAnalyzer(
    Rule.SuppressNuGetIssuesPerPackage,
    Rule.SuppressNuGetAuditIssuesViaNuGetAuditSuppress)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var noWarn in context.File.PropertyGroups.Children<NoWarn>())
        {
            foreach (var ruleId in noWarn.RuleIds)
            {
                if (Issue(ruleId) is { } issue)
                {
                    context.ReportDiagnostic(issue, noWarn, ruleId);
                }
            }
        }
    }

    private static DiagnosticDescriptor? Issue(string ruleId) => ruleId.ToUpperInvariant() switch
    {
        "NU1701" or "NU1702" or "NU1703" => Rule.SuppressNuGetIssuesPerPackage,
        "NU1901" or "NU1902" or "NU1903" or "NU1904" => Rule.SuppressNuGetAuditIssuesViaNuGetAuditSuppress,
        _ => null,
    };
}
