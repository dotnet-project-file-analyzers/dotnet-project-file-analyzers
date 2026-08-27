using DotNetProjectFile.Collections;
using Grammr;

namespace DotNetProjectFile.Json;

public sealed class JsonFalse(SliceSpan span, GrammrTree tree) : JsonValue(span, tree);
