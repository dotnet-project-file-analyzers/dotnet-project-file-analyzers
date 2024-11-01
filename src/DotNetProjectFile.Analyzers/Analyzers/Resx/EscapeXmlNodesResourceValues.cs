namespace DotNetProjectFile.Analyzers.Resx;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class EscapeXmlNodesResourceValues() : ResourceFileAnalyzer(Rule.EscapeXmlNodesResourceValues)
{
    protected override void Register(ResourceFileAnalysisContext context)
    {
        foreach (var data in context.File.Data)
        {
            if (data.Value is { } value && value.Element.Elements().OfType<XElement>().Any())
            {
                context.ReportDiagnostic(Descriptor, value, data.Name);
            }
        }
    }
}
