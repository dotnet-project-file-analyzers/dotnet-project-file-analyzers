using DotNetProjectFile.Collections;
using Grammr;
using static DotNetProjectFile.Json.JsonFileParser;

namespace DotNetProjectFile.Json;

public sealed class JsonString(SliceSpan span, GrammrTree tree) : JsonValue(span, tree)
{
    public string Text => field ??= Tokens.FirstOrDefault(t => t.Kind is Kind.String).ToString()[1..^1];
}
