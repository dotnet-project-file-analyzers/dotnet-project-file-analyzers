namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.NoneItemWithRemoveIsRedudent"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class NoneItemWithRemoveIsRedudent() : MsBuildProjectFileAnalyzer(Rule.NoneItemWithRemoveIsRedudent)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach(var none in context.File.ItemGroups
            .SelectMany(g => g.BuildActions)
            .OfType<None>()
            .Where(n => n.Remove.Any()))
        {
            context.ReportDiagnostic(Descriptor, none, string.Concat(';', none.Remove));
        }
    }
}
