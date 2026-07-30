#pragma warning disable S1210 // We only care about sorting.
using Microsoft.CodeAnalysis;
using System.Text;

namespace DotNetProjectFile.Diagnostics;

/// <summary>Represents an entry that sets the dotnet_diagnostic severity.</summary>
public sealed record GlobalConfigurationEntry() : IComparable<GlobalConfigurationEntry>
{
    /// <summary>Initializes a new instance of the <see cref="GlobalConfigurationEntry"/> class.</summary>
    public GlobalConfigurationEntry(string id, GlobalConfigSeverity severity, string? justification = null) : this()
    {
        Id = new(id);
        Severity = severity;
        Justification = justification;
    }

    /// <inheritdoc cref="DiagnosticDescriptor.Id"/>
    public DiagnosticId Id { get; init; }

    /// <summary>The severity of the diagnostic.</summary>
    public GlobalConfigSeverity Severity { get; init; }

    /// <inheritdoc cref="DiagnosticDescriptor.Title"/>
    public LocalizableString? Title { get; init; }

    /// <summary>The (optional) justification for the override.</summary>
    public string? Justification { get; init; }

    /// <summary>Indicates if this the override of a default.</summary>
    public bool IsOverride { get; init; }

    /// <inheritdoc />
    [Pure]
    public int CompareTo(GlobalConfigurationEntry? other) => other switch
    {
        null => +1,
        _ when IsOverride != other.IsOverride => other.IsOverride.CompareTo(IsOverride),

        _ when Id.Prefix is { } pre_this
            && Id.Numeric is { } n_this
            && other.Id.Prefix is { } pre_othr
            && other.Id.Numeric is { } n_othr
            && pre_this == pre_othr
            => Severity != other.Severity
                ? Severity.CompareTo(other.Severity)
                : n_this.CompareTo(n_othr),

        _ => Id.CompareTo(other.Id),
    };

    /// <inheritdoc />
    [Pure]
    public override string ToString()
    {
        var sb = new StringBuilder();

        var param = $"dotnet_diagnostic.{Id}.severity";

        sb.Append($"{param,-35} = {Severity.ToString().ToLowerInvariant(),-10}");

        if (Title is { })
        {
            sb.Append($" # {Title}");
        }
        if (Justification is { Length: > 0 })
        {
            if (Title is null)
            {
                sb.Append(" #");
            }
            sb.Append($" [Justification: {Justification}]");
        }
        return sb.ToString();
    }

    /// <summary>Creates a new entry based on the <see cref="DiagnosticInfo"/>.</summary>
    [Pure]
    public static GlobalConfigurationEntry From(DiagnosticInfo info) => new()
    {
        Id = info.Id,
        Severity = info.GlobalConfigSeverity,
        Title = info.Title,
    };

    /// <summary>As none (disabled).</summary>
    [Pure]
    public static GlobalConfigurationEntry NON(string id, string? justification = null) => new(id, GlobalConfigSeverity.None, justification);

    /// <summary>As silent (suggesetion).</summary>
    [Pure]
    public static GlobalConfigurationEntry SIL(string id, string? justification = null) => new(id, GlobalConfigSeverity.Silent, justification);

    /// <summary>As info/suggestion.</summary>
    [Pure]
    public static GlobalConfigurationEntry SUG(string id, string? justification = null) => new(id, GlobalConfigSeverity.Suggestion, justification);

    /// <summary>As warning.</summary>
    [Pure]
    public static GlobalConfigurationEntry WRN(string id, string? justification = null) => new(id, GlobalConfigSeverity.Warning, justification);

    /// <summary>As error.</summary>
    [Pure]
    public static GlobalConfigurationEntry ERR(string id, string? justification = null) => new(id, GlobalConfigSeverity.Error, justification);
}
