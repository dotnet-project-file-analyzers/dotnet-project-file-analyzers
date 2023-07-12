namespace DotNetProjectFile.Analyzers.Resx;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class AddInvariantResources : ResourceFileAnalyzer
{
    public AddInvariantResources() : base(Rule.AddInvariantResources) { }

    protected override void Register(ResourceFileAnalysisContext context)
    {
        if (!context.Resource.ForInvariantCulture && context.Resource.Parents.None(p => p.ForInvariantCulture))
        {
            context.ReportDiagnostic(Descriptor, context.Resource, context.Resource.Culture);
        }
    }
}
