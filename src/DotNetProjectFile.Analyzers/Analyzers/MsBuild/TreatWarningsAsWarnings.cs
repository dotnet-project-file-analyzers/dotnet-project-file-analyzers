namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.TreatWarningsAsWarnings"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class TreatWarningsAsWarnings() : MsBuildProjectFileAnalyzer(Rule.TreatWarningsAsWarnings)
{
    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var property in context.File.PropertyGroups
            .Children<TreatWarningsAsErrors>().Where(p => p.Value is true))
        {
            context.ReportDiagnostic(Descriptor, property);
        }
    }
}
