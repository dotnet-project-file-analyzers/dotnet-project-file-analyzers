namespace Grammr;

public static class TokenExtensions
{
    extension(IEnumerable<Token> tokens)
    {
        public bool HasNoneOfKind(string kind) => tokens.None(t => t.Kind == kind);

        public Token? TryOfKind(string kind) => tokens.FirstOrNone(t => t.Kind == kind);

        public Token OfKind(string kind) => tokens.First(t => t.Kind == kind);

        public Token OfAnyKind(params string[] kinds) => tokens.First(t => kinds.Contains(t.Kind));

        public IEnumerable<Token> WhereOfKind(string kind) => tokens.Where(t => t.Kind == kind);
    }
}
