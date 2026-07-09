using DotNetProjectFile.Collections;
using Grammr;
using Microsoft.CodeAnalysis.Text;
using static Grammr.Lexers.Shared;

namespace Specs.GrammrNode_specs;

public class Spans
{
    [Test]
    public void Matches_Line_Position_span()
    {
        var word = str("word");
        var tree = Test.Tree("""
            first line
              word somewhere
            """);
        var reader = new SourceReader(tree.SourceText);
        reader.Keep(line());
        reader.Keep(eol);
        reader.Keep(ws);
        reader.Keep(word);
        reader.Keep(line());
        tree.Finalize(reader.Stream);
        
        var node = new Node(new(0, 5), tree);
        var token = tree.Stream[3];
        var span = node.Spans[token];
        var text = tree.SourceText.TextSpan(span);

        token.ToString().Should().Be("word");
        tree.SourceText.GetSubText(text).ToString().Should().Be("word");
    }


    private sealed class Node(SliceSpan span, GrammrTree tree) : GrammrNode(span, tree);
}
