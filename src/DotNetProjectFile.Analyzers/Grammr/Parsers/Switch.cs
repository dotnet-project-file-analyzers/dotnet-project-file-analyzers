using Grammr.Text;

namespace Grammr.Parsers;

internal sealed class Switch(ImmutableArray<Parser> options) : Parser
{
    private readonly ImmutableArray<Parser> Options = options
        .SelectMany(p => p is Switch s ? s.Options : [p])
        .ToImmutableArray();

    /// <inheritdoc />
    public override ResultQueue Parse(TokenStream stream, ResultQueue queue)
    {
        var temp = new ResultQueue();

        foreach (var option in Options)
        {
            var results = option.Parse(stream, temp.Clear());
            queue.NoMatch(results.Failure.Stream, results.Failure.Message);

            foreach (var result in results.DequeueAll())
            {
                queue.Enqueue(result);
            }
        }
        return queue;
    }
}
