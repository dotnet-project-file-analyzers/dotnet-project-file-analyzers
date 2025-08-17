namespace DotNetProjectFile.Analyzers.Resx;

/// <summary>Implements <see cref="Rule.RESX.AddInvariantFallbackResources"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class AddInvariantFallbackResources() : ResourceFileAnalyzer(Rule.RESX.AddInvariantFallbackResources)
{
    /// <inheritdoc />
    protected override void Register(ResourceFileAnalysisContext context)
    {
        if (!context.File.ForInvariantCulture && context.File.Parents.None(p => p.ForInvariantCulture))
        {
            context.ReportDiagnostic(Descriptor, context.File, context.File.Culture);
        }
    }
}
