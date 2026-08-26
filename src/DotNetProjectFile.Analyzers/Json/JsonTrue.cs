using DotNetProjectFile.Collections;
using Grammr;

namespace DotNetProjectFile.Json;

public sealed class JsonTrue(SliceSpan span, GrammrTree tree) : JsonValue(span, tree);
