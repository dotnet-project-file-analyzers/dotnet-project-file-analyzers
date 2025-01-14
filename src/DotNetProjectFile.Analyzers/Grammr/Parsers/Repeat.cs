using DotNetProjectFile.Collections;
using Grammr.Text;

namespace Grammr.Parsers;

public sealed class Repeat(Parser parser, int minOccurs, int maxOccurs) : Parser
{
    private readonly Parser Parser = parser;
    private readonly int MinOccurs = minOccurs;
    private readonly int MaxOccurs = maxOccurs;

    /// <summary>Only matches all possible matches.</summary>
    public Parser Greedy => new Maximizer(this);

    public override ResultQueue Parse(TokenStream stream, ResultQueue queue)
    {
        if (MinOccurs == 0)
        {
            queue.Match(stream, null);
        }

        var temp = new ResultQueue();
        var currs = new List<Result>() { Result.Match(stream) };
        var nexts = new List<Result>();
        var nodes = AppendOnlyList<Syntax.Node>.Empty;
        var occurs = 0;

        while (occurs < MaxOccurs && currs.Any())
        {
            nexts.Clear();

            foreach (var curr in currs)
            {
                var temps = Parser.Parse(curr.Stream, temp.Clear());

                if (occurs < MinOccurs)
                {
                    queue.NoMatch(temps.Failure.Stream, temp.Failure.Message);
                }

                foreach (var next in temps.DequeueAll())
                {
                    nodes = nodes.Add(next.Node);
                    var node = Select(nodes);
                    nexts.Add(Result.Match(next.Stream, node));

                    // It is enough.
                    if (occurs >= MinOccurs)
                    {
                        queue.Match(next.Stream, node);
                    }
                }
            }

            (currs, nexts) = (nexts, currs);
            occurs++;
        }
        return queue;
    }

    private sealed class Maximizer(Repeat repeat) : Parser
    {
        public override ResultQueue Parse(TokenStream stream, ResultQueue queue)
        {
            var temp = new ResultQueue();
            var currs = new List<Result>() { Result.Match(stream) };
            var nexts = new List<Result>();
            var nodes = AppendOnlyList<Syntax.Node>.Empty;
            var occurs = 0;

            while (occurs < repeat.MaxOccurs && currs.Any())
            {
                nexts.Clear();
                foreach (var curr in currs)
                {
                    var temps = repeat.Parser.Parse(curr.Stream, temp.Clear());

                    if (occurs < repeat.MinOccurs)
                    {
                        queue.NoMatch(temps.Failure.Stream, temp.Failure.Message);
                    }

                    foreach (var next in temps.DequeueAll())
                    {
                        nodes = nodes.Add(next.Node);
                        var node = Select(nodes);
                        nexts.Add(Result.Match(next.Stream, node));
                    }
                }

                (currs, nexts) = (nexts, currs);
                occurs++;
            }

            foreach (var next in nexts)
            {
                queue.Match(next.Stream, next.Node);
            }

            return queue;
        }
    }
}
