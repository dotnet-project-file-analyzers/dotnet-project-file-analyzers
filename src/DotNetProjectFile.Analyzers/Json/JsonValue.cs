using DotNetProjectFile.Collections;
using Grammr;

namespace DotNetProjectFile.Json;

public abstract class JsonValue(SliceSpan span, GrammrTree tree) : GrammrNode(span, tree);
