using DotNetProjectFile.Collections;
using Grammr;

namespace DotNetProjectFile.Json;

public sealed class JsonNull(SliceSpan span, GrammrTree tree) : JsonValue(span, tree)
{
}
