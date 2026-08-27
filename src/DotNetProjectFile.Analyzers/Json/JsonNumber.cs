using DotNetProjectFile.Collections;
using Grammr;
using static DotNetProjectFile.Json.JsonFileParser;

namespace DotNetProjectFile.Json;

public sealed class JsonNumber(SliceSpan span, GrammrTree tree) : JsonValue(span, tree)
{
    public string Text => field ??= Tokens.FirstOrDefault(t => t.Kind is Kind.Number).ToString();
}
