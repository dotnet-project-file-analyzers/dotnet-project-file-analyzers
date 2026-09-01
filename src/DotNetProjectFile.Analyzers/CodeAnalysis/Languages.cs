namespace DotNetProjectFile.CodeAnalysis;

public static class Languages
{
    /// <summary>All languages.</summary>
    public static readonly ImmutableArray<Language> All = [Language.CSharp, Language.FSharp, Language.VisualBasic, Language.VisualCobol];

    /// <summary>C#.</summary>
    public static readonly ImmutableArray<Language> CSharp = [Language.CSharp];

    /// <summary>F#.</summary>
    public static readonly ImmutableArray<Language> FSharp = [Language.FSharp];

    /// <summary>All Roslyn based languages.</summary>
    public static readonly ImmutableArray<Language> RoslynBased = [.. All.Where(l => l.IsRoslynBased)];
}
