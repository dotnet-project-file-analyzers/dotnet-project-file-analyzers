using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class OmitXmlDeclarations() : MsBuildProjectFileAnalyzer(Rule.OmitXmlDeclarations)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.File.Element.Document.Declaration is { })
        {
            var span = new LinePositionSpan(default, context.File.Positions.StartElement.Start);
            context.ReportDiagnostic(Descriptor, span);
        }
    }
}
