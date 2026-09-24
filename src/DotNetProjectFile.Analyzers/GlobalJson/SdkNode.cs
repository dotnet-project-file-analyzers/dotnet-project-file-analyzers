using DotNetProjectFile.Json;
using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.GlobalJson;

public sealed class SdkNode(JsonObject root)
{
    private readonly JsonObject Root = root;

    public LinePositionSpan Span => Root.LinePositionSpan;

    public JsonValue? Version => Root.Property<JsonValue>("version");

    public JsonValue? RollForward => Root.Property<JsonValue>("rollForward");

    public JsonValue? AllowPrerelease => Root.Property<JsonValue>("allowPrerelease");
}
