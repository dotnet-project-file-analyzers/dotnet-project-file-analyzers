namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>
/// Implements <see cref="Rule.SuppressNuGetIssuesPerPackage"/>
/// and <see cref="Rule.SuppressNuGetAdvisoriesPerVulnerability"/>.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class SuppressNuGetViolationsPerCase() : MsBuildProjectFileAnalyzer(
    Rule.SuppressNuGetIssuesPerPackage,
    Rule.SuppressNuGetAdvisoriesPerVulnerability)
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

        foreach (var package in context.File.ItemGroups.Children<PackageReference>())
        {
            foreach (var ruleId in package.NoWarn)
            {
                if (Advisory(ruleId) is { } issue)
                {
                    context.ReportDiagnostic(issue, package, ruleId);
                }
            }
        }
    }

    private static DiagnosticDescriptor? Issue(string ruleId)
        => ruleId.ToUpperInvariant() is "NU1701" or "NU1702" or "NU1703"
        ? Rule.SuppressNuGetIssuesPerPackage
        : Advisory(ruleId);

    private static DiagnosticDescriptor? Advisory(string ruleId)
        => ruleId.ToUpperInvariant() is "NU1901" or "NU1902" or "NU1903" or "NU1904"
        ? Rule.SuppressNuGetSAdvisoriesPerVulnerability
        : null;
}
