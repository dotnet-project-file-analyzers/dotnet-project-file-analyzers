using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

namespace Antlr4;

public class AbstractSyntaxTree(ITokenStream stream, IVocabulary vocabulary)
{
    private readonly ITokenStream Stream = stream;

    public IVocabulary Vocabulary { get; } = vocabulary;

    public IReadOnlyCollection<StreamToken> Tokens(Interval interval)
        => Enumerable.Range(interval.a, interval.Length)
        .Select(i => Stream.Get(i))
        .Select(t => new StreamToken(t.Text, Vocabulary.GetDisplayName(t.Type)))
        .ToArray();

}
