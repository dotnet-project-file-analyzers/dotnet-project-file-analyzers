#pragma warning disable S1210 // We only care about sorting
using NuGet.Versioning;
using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace DotNetProjectFile.Diagnostics;

/// <summary>Info about a NuGet package.</summary>
[DebuggerDisplay("{ToString(),nq}, Count = {Rules.Length}")]
public sealed record NugetPackage : IComparable<NugetPackage>
{
    /// <summary>Gets the ID of the package.</summary>
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    /// <summary>Gets the current (to our knownlegde) version.</summary>
    [JsonPropertyName("v")]
    [JsonConverter(typeof(Json.NuGetVersionJsonConverter))]
    public NuGetVersion? Version { get; init; }

    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    [JsonPropertyName("rules")]
    public ImmutableArray<DiagnosticInfo> Rules { get; init; } = [];

    [Pure]
    internal NugetPackage Save() => this with { Rules = [.. Rules.Select(r => r.Save(Version))] };

    [Pure]
    internal NugetPackage Load() => this with { Rules = [.. Rules.Select(r => r.Load(Version))] };

    /// <inheritdoc />
    [Pure]
    public int CompareTo(NugetPackage? other)
        => other is null
        ? +1
        : Id.CompareTo(other.Id, StringComparison.Ordinal);

    /// <inheritdoc />
    [Pure]
    public override string ToString()
        => Version is null
        ? Id
        : $"{Id} v{Version}";
}
