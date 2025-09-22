using System.Collections.Frozen;

namespace DotNetProjectFile.CodeAnalysis;

/// <summary>
/// Represents a language that is supported by the .NET project file analyzers.
/// </summary>
public readonly struct Language
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly string Id;

    private Language(string id) => Id = id;

    /// <summary>None/unknown langauge.</summary>
    public static readonly Language None;

    /// <summary>C#.</summary>
    public static readonly Language CSharp = new(nameof(CSharp));

    /// <summary>Visual Basic (.NET).</summary>
    public static readonly Language VisualBasic = new(nameof(VisualBasic));

    /// <summary>F#.</summary>
    public static readonly Language FSharp = new(nameof(FSharp));

    /// <summary>All Roslyn based languages.</summary>
    public static IReadOnlyCollection<Language> RoslynBased { get; } = [CSharp, FSharp];

    /// <summary>All languages.</summary>
    public static IReadOnlyCollection<Language> All { get; } = [CSharp, FSharp, VisualBasic];

    /// <summary>Gets the MSBuild project file extenion for the language.</summary>
    public string? ProjectFile => Id switch
    {
        nameof(CSharp) => ".csproj",
        nameof(VisualBasic) => ".vbproj",
        nameof(FSharp) => ".fsproj",
        _ => null,
    };

    /// <summary>Gets the name of the language.</summary>
    public string Name => Id switch
    {
        nameof(CSharp) => "C#",
        nameof(VisualBasic) => "Visual Basic",
        nameof(FSharp) => "F#",
        _ => Id ?? string.Empty,
    };

    /// <inheritdoc />
    public override string ToString() => Name;

    /// <summary>Parses the language.</summary>
    public static Language Parse(string? str)
        => lookup.TryGetValue((str ?? string.Empty).Trim().Replace(" ", string.Empty), out var id)
        ? new(id!)
        : throw new FormatException($"'{str}' is not a valid language");

    private static readonly FrozenDictionary<string, string?> lookup = new Dictionary<string, string?>()
    {
        [string.Empty] = null,

        ["C#"] = nameof(CSharp),
        [".csproj"] = nameof(CSharp),
        [nameof(CSharp)] = nameof(CSharp),

        ["F#"] = nameof(FSharp),
        [".fsproj"] = nameof(FSharp),
        [nameof(FSharp)] = nameof(FSharp),

        ["VB"] = nameof(VisualBasic),
        ["VB.NET"] = nameof(VisualBasic),
        [".vbproj"] = nameof(VisualBasic),
        [nameof(VisualBasic)] = nameof(VisualBasic),
    }
    .ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);
}
