namespace DotNetProjectFile.Analyzers.Slnx;

/// <summary>Implements <see cref="Rule.OmitXmlDeclarations"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class OmitXmlDeclarations() : SolutionFileAnalyzer(Rule.OmitXmlDeclarations)
{
    /// <inheritdoc />
    protected override void Register(SolutionFileAnalysisContext context)
    {
        if (context.File.Element.Document.Declaration is { })
        {
            context.ReportDiagnostic(Descriptor, context.File.Locations.StartElement);
        }
    }
}
