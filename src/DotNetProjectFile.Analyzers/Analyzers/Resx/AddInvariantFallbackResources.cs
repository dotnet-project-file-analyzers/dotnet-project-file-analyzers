namespace DotNetProjectFile.Analyzers.Resx;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class AddInvariantFallbackResources : ResourceFileAnalyzer
{
    public AddInvariantFallbackResources() : base(Rule.AddInvariantFallbackResources) { }

    protected override void Register(ResourceFileAnalysisContext context)
    {
        if (!context.Resource.ForInvariantCulture && context.Resource.Parents.None(p => p.ForInvariantCulture))
        {
            context.ReportDiagnostic(Descriptor, context.Resource, context.Resource.Culture);
        }
    }
}
