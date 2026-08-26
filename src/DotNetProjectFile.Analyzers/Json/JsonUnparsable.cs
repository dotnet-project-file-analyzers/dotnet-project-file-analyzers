using DotNetProjectFile.Collections;
using Grammr;

namespace DotNetProjectFile.Json;

public sealed class JsonUnparsable(SliceSpan span, GrammrTree tree) : GrammrNode(span, tree);
