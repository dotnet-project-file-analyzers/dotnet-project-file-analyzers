using System.IO;
using System.Xml;

namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class PlaceXmlNodesOnSeperateLines() : MsBuildProjectFileAnalyzer(Rule.PlaceXmlNodesOnSeperateLines)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        var nodes = GetFlatChildren(context.Project).ToArray();
        var occupied = new Dictionary<int, Node>();

        foreach (var node in nodes)
        {
            (var startLine, var endLine) = GetLineNumbers(node);

            if (occupied.TryGetValue(startLine, out var prev) && prev != node)
            {
                context.ReportDiagnostic(Descriptor, node, node.ToString());
                continue;
            }
            else
            {
                occupied[startLine] = node;
            }

            if (occupied.TryGetValue(endLine, out prev) && prev != node)
            {
                context.ReportDiagnostic(Descriptor, node, node.ToString());
            }
            else
            {
                occupied[endLine] = node;
            }
        }
    }

    private static IEnumerable<Node> GetFlatChildren(Node node)
    {
        foreach (var child in node.Children)
        {
            yield return child;
            foreach (var grandchild in GetFlatChildren(child))
            {
                yield return grandchild;
            }
        }
    }

#pragma warning disable S3776 // Max complexity of 15. Justification: Logic state machine relatively well contained.
    private static (int StartLine, int EndLine) GetLineNumbers(Node node)
    {
        using XmlReader reader = node.Element.CreateReader();

        if (reader is not IXmlLineInfo lineInfo)
        {
            return (node.LineInfo.LineNumber, node.LineInfo.LineNumber);
        }

        var startLine = -1;
        var endLine = -1;
        var depth = 0;

        while (reader.Read())
        {
            if (reader.LocalName != node.Element.Name.LocalName)
            {
                continue;
            }

            if (reader.NodeType == XmlNodeType.Element)
            {
                if (depth == 0)
                {
                    startLine = lineInfo.LineNumber;

                    if (reader.IsEmptyElement)
                    {
                        return (startLine, startLine);
                    }
                }

                depth++;
            }
            else if (reader.NodeType == XmlNodeType.EndElement)
            {
                depth--;

                if (depth == 0)
                {
                    endLine = lineInfo.LineNumber;
                    break;
                }
            }
        }

        return (startLine, endLine);
    }
#pragma warning restore S3776
}
