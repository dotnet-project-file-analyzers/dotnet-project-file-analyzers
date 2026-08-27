using DotNetProjectFile.Collections;
using Grammr;

namespace DotNetProjectFile.Json;

public sealed class JsonArray(SliceSpan span, GrammrTree tree) : JsonValue(span, tree)
{
    public GrammrNodes<JsonValue> Items => new(Children);
}
