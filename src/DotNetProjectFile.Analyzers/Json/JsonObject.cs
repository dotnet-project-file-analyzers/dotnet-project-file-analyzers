using DotNetProjectFile.Collections;
using Grammr;

namespace DotNetProjectFile.Json;

public sealed class JsonObject(SliceSpan span, GrammrTree tree) : JsonValue(span, tree)
{
    public GrammrNodes<JsonProperty> Properties => new(Children);
}
