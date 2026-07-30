using Qowaiv.Json;

namespace DotNetProjectFile.Diagnostics.Json;

internal sealed class DiagnosticIdJsonConverter : SvoJsonConverter<DiagnosticId>
{
    /// <inheritdoc />
    [Pure]
    protected override DiagnosticId FromJson(string? json) => json is { Length: > 0 } ? new(json) : default;

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(DiagnosticId svo) => svo.Equals(DiagnosticId.Empty) ? null : svo.ToString();
}
