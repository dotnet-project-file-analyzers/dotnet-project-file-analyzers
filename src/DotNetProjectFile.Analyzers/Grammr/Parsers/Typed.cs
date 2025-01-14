using Grammr.Text;

namespace Grammr.Parsers;

[DebuggerDisplay("Typed: {Type.Name}")]
public sealed class Typed : Parser
{
    internal Typed(Parser parser, Type type)
    {
        Parsers = parser;
        Type = type;

        if (!Transformers.TryGetValue(type, out var transformer))
        {
            var ctor = type.GetConstructor([typeof(Grammr.Syntax.Node)]);

            transformer = (node) => node is null ? null : (Syntax.Node?)ctor.Invoke([node]);
            Transformers[type] = transformer;
        }
        Transformer = transformer;
    }

    private readonly Parser Parsers;
    private readonly Type Type;
    private readonly Func<Syntax.Node?, Syntax.Node?> Transformer;

    /// <inheritdoc />
    public override ResultQueue Parse(TokenStream stream, ResultQueue queue)
        => Parsers.Parse(stream, queue).Transform(Transformer);

    private static readonly Dictionary<Type, Func<Syntax.Node?, Syntax.Node?>> Transformers = [];
}
