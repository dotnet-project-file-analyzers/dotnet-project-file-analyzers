namespace DotNetProjectFile.Analyzers.NuGetConfig;

/// <summary>Implements <see cref="Rule.OmitXmlDeclarations"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class OmitXmlDeclarations() : NuGetConfigFileAnalyzer(Rule.OmitXmlDeclarations)
{
    /// <inheritdoc />
    protected override void Register(NuGetConfigFileAnalysisContext context)
    {
        if (context.File.Element.Document.Declaration is { })
        {
            context.ReportDiagnostic(Descriptor, context.File.Locations.StartElement);
        }
    }
}
