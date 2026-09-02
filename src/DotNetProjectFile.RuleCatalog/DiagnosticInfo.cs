#pragma warning disable S1210 // We only care about sorting
using DotNetProjectFile.RuleCatalog.Json;
using Microsoft.CodeAnalysis;
using NuGet.Versioning;
using System.Collections.Immutable;
using System.Globalization;
using System.Text.Json.Serialization;

namespace DotNetProjectFile.RuleCatalog;

/// <summary>
/// Represents <see cref="DiagnosticDescriptor"/> data combined with package,
/// version and language data.
/// </summary>
[DebuggerDisplay("{Id}: {Title}")]
public sealed record DiagnosticInfo :
    IEquatable<DiagnosticInfo>,
    IComparable<DiagnosticInfo>
{
    /// <summary>The (first) version of the diagnostic.</summary>
    [JsonConverter(typeof(NuGetVersionJsonConverter))]
    [JsonPropertyName("f")]
    public NuGetVersion? First { get; init; }

    /// <summary>The (latest) version of the diagnostic.</summary>
    [JsonConverter(typeof(NuGetVersionJsonConverter))]
    [JsonPropertyName("v")]
    public NuGetVersion? Version { get; init; }

    /// <summary>The language of the diagnostic.</summary>
    [JsonConverter(typeof(StringArrayJsonConverter))]
    [JsonPropertyName("lang")]
    public ImmutableArray<string> Languages { get; init; }

    /// <inheritdoc cref="DiagnosticDescriptor.Id" />
    [JsonPropertyName("id")]
    public required DiagnosticId Id { get; init; }

    /// <inheritdoc cref="DiagnosticDescriptor.Title" />
    [JsonPropertyName("title")]
    public string? Title { get; init; }

    /// <inheritdoc cref="DiagnosticDescriptor.Description" />
    [JsonPropertyName("desc")]
    public string? Description { get; init; }

    /// <inheritdoc cref="DiagnosticDescriptor.HelpLinkUri" />
    [JsonPropertyName("url")]
    public string? HelpLinkUri { get; init; }

    /// <inheritdoc cref="DiagnosticDescriptor.CustomTags" />
    [JsonConverter(typeof(StringArrayJsonConverter))]
    [JsonPropertyName("tags")]
    public ImmutableArray<string> CustomTags { get; init; } = [];

    /// <inheritdoc cref="DiagnosticDescriptor.DefaultSeverity" />
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [JsonPropertyName("level")]
    public DiagnosticSeverity DefaultSeverity { get; init; } = DiagnosticSeverity.Warning;

    /// <inheritdoc cref="DiagnosticDescriptor.IsEnabledByDefault" />
    [JsonPropertyName("enabled")]
    public bool IsEnabledByDefault { get; init; } = true;

    /// <summary>The .globalconfig severity.</summary>
    [JsonIgnore]
    public GlobalConfigSeverity GlobalConfigSeverity => DefaultSeverity switch
    {
        _ when !IsEnabledByDefault /*.*/ => GlobalConfigSeverity.None,
        DiagnosticSeverity.Hidden /*..*/ => GlobalConfigSeverity.Silent,
        DiagnosticSeverity.Info /*....*/ => GlobalConfigSeverity.Suggestion,
        DiagnosticSeverity.Warning /*.*/ => GlobalConfigSeverity.Warning,
        DiagnosticSeverity.Error /*...*/ => GlobalConfigSeverity.Error,
        _ => throw new InvalidCastException($"{DefaultSeverity} can not be mapped to {typeof(GlobalConfigSeverity)}."),
    };

    /// <summary>Obsolete indication.</summary>
    [JsonPropertyName("obsolete")]
    public string? Obsolete { get; init; }

    [Pure]
    public DiagnosticInfo Update(DiagnosticInfo update) => update with
    {
        First = First,
        Title = update.Title.NullIfEmpty() ?? Title,
        Description = update.Description.NullIfEmpty() ?? Description,
        HelpLinkUri = update.HelpLinkUri.NullIfEmpty() ?? HelpLinkUri,
        Obsolete = update.Obsolete.NullIfEmpty() ?? Obsolete,
    };

    [Pure]
    internal DiagnosticInfo Save(NuGetVersion? version) => this with
    {
        Version = Version == version ? null : Version,
        Title = Title.NullIfEmpty(),
        Description = Description.NullIfEmpty(),
        HelpLinkUri = HelpLinkUri.NullIfEmpty(),
        Obsolete = Obsolete.NullIfEmpty(),
    };

    [Pure]
    internal DiagnosticInfo Load(NuGetVersion? version) => this with { Version = Version ?? version };

    /// <inheritdoc />
    [Pure]
    public int CompareTo(DiagnosticInfo? other) => other switch
    {
        null => +1,

        // Same Prefix, and a both a numeric part
        _ when Id.Prefix is { } pre_this
            && Id.Numeric is { } n_this
            && other.Id.Prefix is { } pre_othr
            && other.Id.Numeric is { } n_othr
            && pre_this == pre_othr
        => n_this.CompareTo(n_othr),

        _ => Id.CompareTo(other.Id),
    };

    /// <inheritdoc />
    [Pure]
    public bool Equals(DiagnosticInfo? other)
        => other is { }
        && Id == other.Id;

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => Id.GetHashCode();

    /// <summary>
    /// Creates a new <see cref="DiagnosticInfo"/> based on the <see cref="DiagnosticDescriptor"/>.
    /// </summary>
    /// <param name="descriptor">
    /// The matching <see cref="DiagnosticDescriptor"/>.
    /// </param>
    [Pure]
    public static DiagnosticInfo New(DiagnosticDescriptor descriptor) => new()
    {
        Id = new(descriptor.Id),
        Title = descriptor.Title.ToString(CultureInfo.InvariantCulture),
        Description = descriptor.Description.ToString(CultureInfo.InvariantCulture),
        CustomTags = [.. descriptor.CustomTags],
        DefaultSeverity = descriptor.DefaultSeverity,
        IsEnabledByDefault = descriptor.IsEnabledByDefault,
        HelpLinkUri = descriptor.HelpLinkUri,
    };
}
