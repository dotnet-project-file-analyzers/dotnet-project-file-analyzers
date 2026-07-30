using Qowaiv.Diagnostics.Contracts;
using System.Collections.Immutable;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DotNetProjectFile.Diagnostics;

/// <summary>A collection of <see cref="NugetPackage"/>s.</summary>
[DebuggerDisplay("Packages = {Packages.Length}, Rules = {Count}")]
public sealed record DiagnosticCollection
{
    /// <summary>Rules that are similar.</summary>
    [JsonPropertyName("similar")]
    public ImmutableArray<ImmutableArray<DiagnosticId>> Similar { get; init; } = [];

    [JsonPropertyName("packages")]
    public ImmutableArray<NugetPackage> Packages { get; init; } = [];

    /// <summary>The number of rules combined.</summary>
    public int Count => Rules.Count();

    /// <summary>All rules.</summary>
    [JsonIgnore]
    public IEnumerable<DiagnosticInfo> Rules => Packages.SelectMany(p => p.Rules);

    /// <summary>Gets the rules from the specified packages.</summary>
    /// <param name="packageIds">
    /// To select from.
    /// </param>
    [Pure]
    public IEnumerable<DiagnosticInfo> FromPackages(params IReadOnlyCollection<string> packageIds)
        => Packages.Where(p => packageIds.Contains(p.Id))
        .SelectMany(p => p.Rules);

    /// <summary>Saves the collection as JSON.</summary>
    /// <param name="stream">
    /// The stream to save to.
    /// </param>
    public void Save(Stream stream)
        => JsonSerializer.Serialize(stream, this, options: Options);

    /// <summary>Loads the collection from a JSON stream.</summary>
    /// <param name="stream">
    /// The stream to load from.
    /// </param>
    [Impure]
    public static DiagnosticCollection Load(Stream stream)
        => JsonSerializer.Deserialize<DiagnosticCollection>(stream, Options)
        ?? throw new JsonException("Could not deserialize the analyzers info.");

    /// <summary>Gets the embedded (pre collected) collection.</summary>
    [Pure]
    public static DiagnosticCollection Embedded()
        => Load(typeof(DiagnosticCollection).Assembly.GetManifestResourceStream("DotNetProjectFile.Diagnostics.Data.DiagnosticCollection.json")
            ?? throw new FileNotFoundException("Embedded resource 'DotNetProjectFile.Diagnostics.Data.DiagnosticCollection.json' not found."));

    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        IndentCharacter = ' ',
        IndentSize = 2,
        NewLine = "\n",
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };
}
