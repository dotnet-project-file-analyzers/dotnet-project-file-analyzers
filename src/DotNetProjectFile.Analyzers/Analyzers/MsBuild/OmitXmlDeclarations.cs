using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class OmitXmlDeclarations() : MsBuildProjectFileAnalyzer(Rule.OmitXmlDeclarations)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.Project.Element.Document.Declaration is { })
        {
            var span = new LinePositionSpan(default, context.Project.Positions.StartElement.Start);
            context.ReportDiagnostic(Description, span);
        }
    }
}
