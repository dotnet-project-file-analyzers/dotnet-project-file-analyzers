using System.Collections.Frozen;
using System.ComponentModel;

namespace DotNetProjectFile.CodeAnalysis;

/// <summary>
/// Represents a language that is supported by the .NET project file analyzers.
/// </summary>
[TypeConverter(typeof(LanguageTypeConverter))]
public readonly struct Language : IEquatable<Language>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly string Id;

    private Language(string id, string name, string xxproj, bool roslynBased)
    {
        Id = id;
        Name = name;
        ProjectFileExtension = xxproj;
        IsRoslynBased = roslynBased;
    }

    /// <summary>None/unknown langauge.</summary>
    public static readonly Language None;

    /// <summary>C#.</summary>
    public static readonly Language CSharp = new(
        nameof(CSharp),
        name: "C#",
        xxproj: ".csproj",
        roslynBased: true);

    /// <summary>Visual Basic (.NET).</summary>
    public static readonly Language VisualBasic = new(
        nameof(VisualBasic),
        name: "Visual Basic",
        xxproj: ".vbproj",
        roslynBased: true);

    /// <summary>F#.</summary>
    public static readonly Language FSharp = new(
        nameof(FSharp),
        name: "F#",
        xxproj: ".fsproj",
        roslynBased: false);

    /// <summary>Visual Cobol</summary>
    public static readonly Language VisualCobol = new(
        nameof(VisualCobol),
        name: "Visual Cobol",
        xxproj: ".cblproj",
        roslynBased: false);

    /// <summary>Gets the name of the language.</summary>
    public readonly string Name;

    /// <summary>Indicates that a language is Roslyn based.</summary>
    public readonly bool IsRoslynBased;

    /// <summary>Gets the MSBuild project file extenion for the language.</summary>
    public readonly string ProjectFileExtension;

    /// <inheritdoc />
    public override string ToString() => Name;

    /// <summary>Parses the language.</summary>
    public static Language Parse(string? str)
        => lookup.TryGetValue((str ?? string.Empty).Trim().Replace(" ", string.Empty), out var lang)
        ? lang
        : None;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Language other && Equals(other);

    /// <inheritdoc />
    public bool Equals(Language other) => Id == other.Id;

    /// <inheritdoc />
    public override int GetHashCode() => Id is null ? 0 : Id.GetHashCode();

    /// <summary>Return true when the languages are equal.</summary>
    public static bool operator ==(Language left, Language right) => left.Equals(right);

    /// <summary>Return true when the languages are different.</summary>
    public static bool operator !=(Language left, Language right) => !(left == right);

    private static readonly FrozenDictionary<string, Language> lookup = new Dictionary<string, Language>()
    {
        ["C#"] = CSharp,
        [".csproj"] = CSharp,
        [nameof(CSharp)] = CSharp,

        ["F#"] = FSharp,
        [".fsproj"] = FSharp,
        [nameof(FSharp)] = FSharp,

        ["VB"] = VisualBasic,
        ["VB.NET"] = VisualBasic,
        [".vbproj"] = VisualBasic,
        [nameof(VisualBasic)] = VisualBasic,
    }
    .ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);
}
