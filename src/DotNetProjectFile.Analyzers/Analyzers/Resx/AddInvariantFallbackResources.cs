namespace DotNetProjectFile.Analyzers.Resx;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class AddInvariantFallbackResources() : ResourceFileAnalyzer(Rule.AddInvariantFallbackResources)
{
    protected override void Register(ResourceFileAnalysisContext context)
    {
        if (!context.Resource.ForInvariantCulture && context.Resource.Parents.None(p => p.ForInvariantCulture))
        {
            context.ReportDiagnostic(Description, context.Resource, context.Resource.Culture);
        }
    }
}
