using System.Collections.Frozen;

namespace DotNetProjectFile.CodeAnalysis;

/// <summary>
/// Represents a language that is supported by the .NET project file analyzers.
/// </summary>
public readonly struct Language : IEquatable<Language>
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

    /// <summary>Indicates that a language is Roslyn based.</summary>
    public bool IsRoslynBased => Id is nameof(CSharp) or nameof(VisualBasic);

    /// <inheritdoc />
    public override string ToString() => Name;

    /// <summary>Parses the language.</summary>
    public static Language Parse(string? str)
        => lookup.TryGetValue((str ?? string.Empty).Trim().Replace(" ", string.Empty), out var id)
        ? new(id!)
        : None;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Language other && Equals(other);

    /// <inheritdoc />
    public bool Equals(Language other) => Id == other.Id;

    /// <summary>Return true when the languages are equal.</summary>
    public static bool operator ==(Language left, Language right) => left.Equals(right);

    /// <summary>Return true when the languages are different.</summary>
    public static bool operator !=(Language left, Language right) => !(left == right);

    private static readonly FrozenDictionary<string, string?> lookup = new Dictionary<string, string?>()
    {
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
