namespace DotNetProjectFile.Analyzers.Resx;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class AddInvariantFallbackResources() : ResourceFileAnalyzer(Rule.AddInvariantFallbackResources)
{
    protected override void Register(ResourceFileAnalysisContext context)
    {
        if (!context.File.ForInvariantCulture && context.File.Parents.None(p => p.ForInvariantCulture))
        {
            context.ReportDiagnostic(Descriptor, context.File, context.File.Culture);
        }
    }
}
