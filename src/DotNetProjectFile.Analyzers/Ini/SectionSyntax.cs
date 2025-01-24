using Antlr4;

namespace DotNetProjectFile.Ini;

public sealed class SectionSyntax(
    HeaderSyntax? header,
    IReadOnlyList<KeyValuePairSyntax> pairs)
{
    public HeaderSyntax? Header { get; } = header;
    public IReadOnlyList<KeyValuePairSyntax> Pairs { get; } = pairs;
}
