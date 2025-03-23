namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.RemoveNoneWhenRedundant"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class RemoveNoneWhenRedundant() : MsBuildProjectFileAnalyzer(Rule.RemoveNoneWhenRedundant)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var none in context.File.ItemGroups
            .Children<None>(n => n.Remove.Any()))
        {
            context.ReportDiagnostic(Descriptor, none, string.Concat(';', none.Remove));
        }
    }
}
