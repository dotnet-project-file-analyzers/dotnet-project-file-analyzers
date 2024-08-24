using Microsoft.CodeAnalysis.Text;
using System.Xml;

namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class IndentXml(char ch, int repeat) : MsBuildProjectFileAnalyzer(Rule.IndentXml)
{
    private readonly char Char = ch;
    private readonly int Repeat = repeat;

    public IndentXml() : this(' ', 2) { }

    protected override void Register(ProjectFileAnalysisContext context)
        => Walk(context.Project, 0, context.Project.Text, context);

    private void Walk(Node node, int depth, SourceText text, ProjectFileAnalysisContext context)
    {
        var line = node.LineInfo.LinePositionSpan();
        var prev = new LinePositionSpan(new(line.Start.Line, 0), new(line.Start.Line, line.Start.Character - 1));
        var span = text.Lines.GetTextSpan(prev);
        var indentation = text.ToString(span);

        if (indentation != new string(Char, Repeat * depth))
        {
            context.ReportDiagnostic(Descriptor, node, node.LocalName);
        }

        foreach (var child in node.Children)
        {
            Walk(child, depth + 1, text, context);
        }
    }
}
