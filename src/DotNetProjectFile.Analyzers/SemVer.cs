using System.ComponentModel;
using System.Globalization;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace DotNetProjectFile;

/// <summary>Implementation of Semantic Versioning.</summary>
/// <remarks>
/// See: https://semver.org/.
/// </remarks>
[TypeConverter(typeof(Conversion.SemVerConverter))]
public sealed record SemVer()
{
    /// <summary>Initializes a new instance of the <see cref="SemVer"/> class.</summary>
    public SemVer(
        BigInteger major,
        BigInteger minor,
        BigInteger patch,
        string? preRelease = null,
        string? buildMetadata = null) : this()
    {
        Major = major;
        Minor = minor;
        Patch = patch;
        PreRelease = preRelease;
        BuildMetadata = buildMetadata;
    }

    /// <summary>Represents the major (should change for incompatible API changes).</summary>
    public BigInteger Major { get; init; }

    /// <summary>Represents the minor (should change for backward compatible added functionality).</summary>
    public BigInteger Minor { get; init; }

    /// <summary>Represents the minor (should change fora backward compatible bug fixes).</summary>
    public BigInteger Patch { get; init; }

    /// <summary>Aditional pre-release label (optional).</summary>
    public string? PreRelease { get; init => field = value.NullIfEmpty(); }

    /// <summary>Aditional build metadata label (optional).</summary>
    public string? BuildMetadata { get; init => field = value.NullIfEmpty(); }

    /// <inheritdoc />
    [Pure]
    public override string ToString()
    {
        var sb = new StringBuilder()
            .Append(Major)
            .Append('.')
            .Append(Minor)
            .Append('.')
            .Append(Patch);

        if (PreRelease is { })
            sb.Append('-').Append(PreRelease);

        if (BuildMetadata is { })
            sb.Append('+').Append(BuildMetadata);

        return sb.ToString();
    }

    /// <summary>Tries to parse a semantic version.</summary>
    [Pure]
    public static SemVer? TryParse(string? s) => s switch
    {
        null or "" => null,
        _ when Pattern.Match(s.Trim()) is { Success: true } match => new()
        {
            Major = BigInteger.Parse(match.Groups[nameof(Major)].Value),
            Minor = BigInteger.Parse(match.Groups[nameof(Minor)].Value),
            Patch = BigInteger.Parse(match.Groups[nameof(Patch)].Value),
            PreRelease = match.Groups[nameof(PreRelease)].Value,
            BuildMetadata = match.Groups[nameof(BuildMetadata)].Value,
        },
        _ => null,
    };

    private static readonly Regex Pattern = new(
        @"^(?<Major>0|[1-9][0-9]*)\.(?<Minor>0|[1-9][0-9]*)\.(?<Patch>0|[1-9][0-9]*)(?:-(?<PreRelease>(?:0|[1-9][0-9]*|[0-9]*[a-zA-Z-][0-9a-zA-Z-]*)(?:\.(?:0|[1-9][0-9]*|[0-9]*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\+(?<BuildMetadata>[0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*))?$",
        RegexOptions.CultureInvariant | RegexOptions.Compiled,
        TimeSpan.FromSeconds(1));
}
