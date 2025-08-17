namespace DotNetProjectFile.Analyzers.Slnx;

/// <summary>Implements <see cref="Rule.SLNX.OmitProjectIds"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class OmitProjectIds() : SolutionFileAnalyzer(Rule.SLNX.OmitProjectIds)
{
    /// <inheritdoc />
    protected override void Register(SolutionFileAnalysisContext context)
    {
        foreach (var project in context.File.Projects.Where(p => p.Id is not null))
        {
            context.ReportDiagnostic(Descriptor, project);
        }
    }
}
