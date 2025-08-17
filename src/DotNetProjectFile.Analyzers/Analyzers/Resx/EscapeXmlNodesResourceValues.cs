namespace DotNetProjectFile.Analyzers.Resx;

/// <summary>Implments <see cref="Rule.RESX.EscapeXmlNodesResourceValues"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class EscapeXmlNodesResourceValues() : ResourceFileAnalyzer(Rule.RESX.EscapeXmlNodesResourceValues)
{
    /// <inheritdoc />
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
