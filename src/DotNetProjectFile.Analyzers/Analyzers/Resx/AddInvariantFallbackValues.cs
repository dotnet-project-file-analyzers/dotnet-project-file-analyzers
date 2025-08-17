namespace DotNetProjectFile.Analyzers.Resx;

/// <summary>Implements <see cref="Rule.RESX.AddInvariantFallbackValues"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class AddInvariantFallbackValues() : ResourceFileAnalyzer(Rule.RESX.AddInvariantFallbackValues)
{
    /// <inheritdoc />
    protected override void Register(ResourceFileAnalysisContext context)
    {
        if (context.File is { ForInvariantCulture: false } resource
            && resource.Parents.FirstOrDefault(p => p.ForInvariantCulture) is { } parent)
        {
            foreach (var data in resource.Data.Where(d => !parent.Contains(d.Name)))
            {
                context.ReportDiagnostic(Descriptor, data, data.Name);
            }
        }
    }
}
