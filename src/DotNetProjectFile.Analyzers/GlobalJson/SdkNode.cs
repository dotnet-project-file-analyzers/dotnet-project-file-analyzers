using DotNetProjectFile.Json;
using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.GlobalJson;

public sealed class SdkNode(JsonObject root)
{
    public JsonObject Root { get; } = root;

    public LinePositionSpan Span => Root.LinePositionSpan;

    public JsonValue? Version => Root.Property<JsonValue>("version");

    public JsonValue? RollForward => Root.Property<JsonValue>("rollForward");

    public JsonValue? AllowPrerelease => Root.Property<JsonValue>("allowPrerelease");

    public RollForwardPolicy RollForwardPolicy
        => RollForward is JsonString value
        && value.Text.All(char.IsLetter)
        && Enum.TryParse<RollForwardPolicy>(value.Text, ignoreCase: true, out var policy)
        ? policy
        : RollForwardPolicy.None;
}
