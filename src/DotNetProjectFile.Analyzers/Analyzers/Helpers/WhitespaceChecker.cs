using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.Analyzers.Helpers;

internal sealed class WhitespaceChecker<TFile>(
    DiagnosticDescriptor descriptor)
    where TFile : class, ProjectFile, XmlAnalysisNode
{
    private readonly DiagnosticDescriptor Descriptor = descriptor;

    public void Check(
       XmlAnalysisNode node,
       SourceText text,
       ProjectFileAnalysisContext<TFile> context)
    {
        foreach (var attr in node.Element.Attributes())
        {
            var pos = AttributesPositions.New(attr, text);

            if (pos.Assignment.Start.Character - pos.Name.End.Character >= 1)
            {
                var span = new LinePositionSpan(pos.Name.End, pos.Assignment.Start);
                context.ReportDiagnostic(Descriptor, context.File, span, "leading");
            }

            if (pos.Value.Start.Character - pos.Assignment.End.Character >= 2)
            {
                var span = new LinePositionSpan(pos.Assignment.End, pos.Value.Start.Expand(-1));
                context.ReportDiagnostic(Descriptor, context.File, span, "trailing");
            }

            var ws = Whitespace.Trim(attr.Value);

            if (ws.HasAny)
            {
                var (leading, trailing) = ws;

                var val = pos.Value;

                if (leading > 0)
                {
                    var span = new LinePositionSpan(val.Start, val.Start.Expand(leading));
                    context.ReportDiagnostic(Descriptor, context.File, span, nameof(leading));
                }
                if (trailing > 0)
                {
                    var span = new LinePositionSpan(val.End.Expand(-trailing), val.End);
                    context.ReportDiagnostic(Descriptor, context.File, span, nameof(trailing));
                }
            }
        }

        foreach (var child in node.Children())
        {
            Check(child, text, context);
        }
    }

    private readonly record struct Whitespace(int Leading, int Trailing)
    {
        public bool HasAny => Leading is not 0 || Trailing is not 0;

        /// <summary>Gets the leading and trailing whitespace (if any).</summary>
        public static Whitespace Trim(string text)
        {
            var leading = 0;
            while (leading < text.Length && char.IsWhiteSpace(text[leading]))
            {
                leading++;
            }
            var trailing = text.Length - 1;
            while (trailing > leading && char.IsWhiteSpace(text[trailing]))
            {
                trailing--;
            }

            return new(leading, text.Length - trailing - 1);
        }
    }
}
