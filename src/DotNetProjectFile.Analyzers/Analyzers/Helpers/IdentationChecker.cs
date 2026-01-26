using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.Analyzers.Helpers;

internal sealed class IdentationChecker<TFile>(
    char ch,
    int repeat,
    DiagnosticDescriptor descriptor,
    Predicate<XmlAnalysisNode>? exclude = null)
    where TFile : class, ProjectFile, XmlAnalysisNode
{
    private readonly char Char = ch;
    private readonly int Repeat = repeat;
    private readonly DiagnosticDescriptor Descriptor = descriptor;
    private readonly Predicate<XmlAnalysisNode> Exclude = exclude ?? (_ => false);

    public void Walk(
        XmlAnalysisNode node,
        SourceText text,
        ProjectFileAnalysisContext<TFile> context)
    {
        if (!Exclude(node))
        {
            Report(node, text, context, true);
            Report(node, text, context, false);
        }
        foreach (var child in node.Children())
        {
            Walk(child, text, context);
        }
    }

    private void Report(XmlAnalysisNode node, SourceText text, ProjectFileAnalysisContext<TFile> context, bool start)
    {
        var positions = node.Locations.Positions;

        var checkSelfClosingEnd = !start && positions.IsSelfClosing;
        var startAndEndOnSameLine = !start && positions.StartElement.End.Line == positions.EndElement.Start.Line;

        if (checkSelfClosingEnd || startAndEndOnSameLine) { return; }

        var element = start ? positions.StartElement : positions.EndElement;

        if (!ProperlyIndented(element) && !ClosingTagAfterTextWithSpacePreservation(node.Element))
        {
            var name = start ? node.Element.Name.LocalName : '/' + node.Element.Name.LocalName;
            context.ReportDiagnostic(Descriptor, start ? node.Locations.StartElement : node.Locations.EndElement, name);
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

        bool ClosingTagAfterTextWithSpacePreservation(XElement element)
            => !start
            && element.Elements().None()
            && element.PreservesSpace();
    }
}
