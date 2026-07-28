#pragma warning disable S1210 // We only care about sorting

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace DotNetProjectFile.Diagnostics;

/// <summary>Represents the Id of the <see cref="Microsoft.CodeAnalysis.DiagnosticDescriptor"/>.</summary>
[JsonConverter(typeof(Json.DiagnosticIdJsonConverter))]
public readonly struct DiagnosticId(string val) : IEquatable<DiagnosticId>, IComparable<DiagnosticId>
{
    /// <summary>Empty/not set diagnostic ID.</summary>
    public static readonly DiagnosticId Empty;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly string value = val;

    /// <summary>The (optional) prefix.</summary>
    public string? Prefix
        => Pattern.Match(value) is { Success: true } match
        ? match.Groups[nameof(Prefix)].Value
        : null;

    /// <summary>The (optional) numeric value.</summary>
    public int? Numeric
        => Pattern.Match(value) is { Success: true } m && int.TryParse(m.Groups[nameof(Numeric)].Value, out var n)
        ? n
        : null;

    /// <inheritdoc />
    [Pure]
    public override string ToString() => value ?? string.Empty;

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => value?.GetHashCode() ?? 0;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is DiagnosticId other
        && Equals(other);

    /// <inheritdoc />
    [Pure]
    public bool Equals(DiagnosticId other) => value == other.value;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(DiagnosticId other) => (value ?? string.Empty).CompareTo(other.value);

    /// <summary>True if left and right are equal.</summary>
    public static bool operator ==(DiagnosticId left, DiagnosticId right) => left.Equals(right);

    /// <summary>True if left and right are not equal.</summary>
    public static bool operator !=(DiagnosticId left, DiagnosticId right) => !(left == right);

    private static readonly Regex Pattern = new(
        "^(?<Prefix>[A-Z]+)(?<Numeric>[0-9]+)",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant,
        TimeSpan.FromMilliseconds(100));
}
