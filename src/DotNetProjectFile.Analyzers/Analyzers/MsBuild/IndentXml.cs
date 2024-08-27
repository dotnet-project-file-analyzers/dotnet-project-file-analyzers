using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class IndentXml(char ch, int repeat) : MsBuildProjectFileAnalyzer(Rule.IndentXml)
{
    private readonly char Char = ch;
    private readonly int Repeat = repeat;

    public IndentXml() : this(' ', 2) { }

    protected override void Register(ProjectFileAnalysisContext context)
        => Walk(context.Project, context.Project, context.Project.Text, context);

    private void Walk(MsBuildProject project, Node node, SourceText text, ProjectFileAnalysisContext context)
    {
        Report(project, node, text, context, true);
        Report(project, node, text, context, false);

        foreach (var child in node.Children)
        {
            Walk(project, child, text, context);
        }
    }

    private void Report(MsBuildProject project, Node node, SourceText text, ProjectFileAnalysisContext context, bool start)
    {
        var checkSelfClosingEnd = !start && node.Positions.IsSelfClosing;
        var startAndEndOnSameLine = !start && node.Positions.StartElement.End.Line == node.Positions.EndElement.Start.Line;

        if (checkSelfClosingEnd || startAndEndOnSameLine) { return; }

        var element = start ? node.Positions.StartElement : node.Positions.EndElement;

        if (!ProperlyIndented(element))
        {
            var name = start ? node.LocalName : '/' + node.LocalName;
            context.ReportDiagnostic(Descriptor, project, element, name);
        }

        bool ProperlyIndented(LinePositionSpan element)
        {
            var width = Repeat * node.Depth;

            if (width != element.Start.Character)
            {
                return false;
            }
            else if (width == 0)
            {
                return true;
            }
            else
            {
                var span = new LinePositionSpan(new(element.Start.Line, 0), new(element.Start.Line, width));
                var indent = text.ToString(text.Lines.GetTextSpan(span));
                return indent.All(ch => ch == Char);
            }
        }
    }
}
