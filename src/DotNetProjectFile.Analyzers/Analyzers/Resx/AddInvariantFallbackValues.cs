namespace DotNetProjectFile.Analyzers.Resx;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class AddInvariantFallbackValues() : ResourceFileAnalyzer(Rule.AddInvariantFallbackValues)
{
    protected override void Register(ResourceFileAnalysisContext context)
    {
        if (context.Resource is { ForInvariantCulture: false } resource
            && resource.Parents.FirstOrDefault(p => p.ForInvariantCulture) is { } parent)
        {
            foreach (var data in resource.Data.Where(d => !parent.Contains(d.Name)))
            {
                context.ReportDiagnostic(Description, data, data.Name);
            }
        }
    }
}
