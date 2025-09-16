using System.Collections.Frozen;

namespace DotNetProjectFile;

public static class Languages
{
    private static readonly FrozenDictionary<ProjectLanguage, string> enumToName
        = new Dictionary<ProjectLanguage, string>
        {
            [ProjectLanguage.CSharp] = LanguageNames.CSharp,
            [ProjectLanguage.FSharp] = LanguageNames.FSharp,
            [ProjectLanguage.VisualBasic] = LanguageNames.VisualBasic,
            [ProjectLanguage.VisualCobol] = LanguageNames.VisualCobol,
        }
        .ToFrozenDictionary();

    private static readonly FrozenDictionary<ProjectLanguage, string> enumToExtension
        = new Dictionary<ProjectLanguage, string>
        {
            [ProjectLanguage.CSharp] = ".csproj",
            [ProjectLanguage.FSharp] = ".fsproj",
            [ProjectLanguage.VisualBasic] = ".vbproj",
            [ProjectLanguage.VisualCobol] = ".cblproj",
        }
        .ToFrozenDictionary();

    private static readonly FrozenDictionary<string, ProjectLanguage> toEnum
        = enumToName
        .Concat(enumToExtension)
        .ToFrozenDictionary(static x => x.Value, static x => x.Key);

    public static string? GetName(this ProjectLanguage language)
        => enumToName.TryGetValue(language, out var result)
        ? result
        : null;

    public static string? GetExtension(this ProjectLanguage language)
        => enumToExtension.TryGetValue(language, out var result)
        ? result
        : null;

    public static ProjectLanguage Parse(this string? str)
        => str is null || !toEnum.TryGetValue(str, out var result)
        ? ProjectLanguage.Unknown
        : result;

    public static bool IsSupportedByRoslyn(this ProjectLanguage language)
        => language is ProjectLanguage.CSharp or ProjectLanguage.VisualBasic;
}
