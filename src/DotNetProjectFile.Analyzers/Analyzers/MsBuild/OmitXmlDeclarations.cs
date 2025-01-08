namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class OmitXmlDeclarations() : MsBuildProjectFileAnalyzer(Rule.OmitXmlDeclarations)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.File.Element.Document.Declaration is { })
        {
            context.ReportDiagnostic(Descriptor, context.File.Locations.StartElement);
        }
    }
}
