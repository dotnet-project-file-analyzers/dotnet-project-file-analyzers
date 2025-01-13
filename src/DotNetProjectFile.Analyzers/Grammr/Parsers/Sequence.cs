using DotNetProjectFile.Collections;
using Grammr.Text;

namespace Grammr.Parsers;

internal sealed class Sequence(ImmutableArray<Parser> sequances) : Parser
{
    private readonly ImmutableArray<Parser> Sequances = sequances
        .SelectMany(p => p is Sequence s ? s.Sequances : [p])
        .ToImmutableArray();

    /// <inheritdoc />
    public override ResultQueue Parse(TokenStream stream, ResultQueue queue)
    {
        var temp = new ResultQueue();
        var currs = new List<Result>();
        var nexts = new List<Result>() { Result.Match(stream) };

        var nodes = AppendOnlyList<Syntax.TreeNode>.Empty;

        foreach (var sequance in Sequances)
        {
            (currs, nexts) = (nexts, currs);
            nexts.Clear();

            foreach (var curr in currs)
            {
                var temps = sequance.Parse(curr.Stream, temp.Clear());
                queue.NoMatch(temps.Failure.Stream, temp.Failure.Message);

                foreach (var next in temps.DequeueAll())
                {
                    nodes = nodes.Add(next.Node);
                    var node = Select(nodes);
                    nexts.Add(Result.Match(next.Stream, node));
                }
            }

            if (sequance == Sequances[^1])
            {
                foreach (var res in nexts)
                {
                    queue.Match(res.Stream, res.Node);
                }
            }
        }
        return queue;
    }
}
