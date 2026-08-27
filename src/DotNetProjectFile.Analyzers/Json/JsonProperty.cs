using DotNetProjectFile.Collections;
using Grammr;

namespace DotNetProjectFile.Json;

public sealed class JsonProperty(SliceSpan span, GrammrTree tree) : GrammrNode(span, tree)
{
    public JsonString? Key => field ??= Children.OfType<JsonString>().FirstOrDefault();

    public JsonValue? Value => field ??= Children.OfType<JsonValue>().FirstOrDefault(v => v != Key);
}
