using DotNetProjectFile.Parsing;

namespace FluentAssertions;

internal static class DotNetProjectFIleFLuentAssertionsExtensions
{
    [Pure]
    public static ParserAssertions Should(this Parser parser) => new(parser);

}
