using DotNetProjectFile.Collections;
using Grammr;
using static DotNetProjectFile.Json.JsonFileParser;

namespace DotNetProjectFile.Json;

public sealed class JsonBoolean(SliceSpan span, GrammrTree tree) : JsonValue(span, tree)
{
    private bool? value;

    public bool Value => value ??= Tokens.FirstOrDefault(t => t.Kind is Kind.True or Kind.False).Kind == Kind.True;
}
