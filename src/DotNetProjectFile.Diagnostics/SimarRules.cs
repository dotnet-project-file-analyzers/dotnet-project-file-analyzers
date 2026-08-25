using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace DotNetProjectFile.Diagnostics;

public sealed record SimarRules
{
    [JsonPropertyName("desc")]
    public required string Description { get; init; }

    [JsonPropertyName("rules")]
    public ImmutableArray<DiagnosticId> Rules { get; init; } = [];
}
