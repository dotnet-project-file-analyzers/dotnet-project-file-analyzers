namespace DotNetProjectFile.Ini;

public static class IniFileTypes
{
    public static readonly ImmutableArray<AnalyzerType> EditorConfig = [AnalyzerType.EditorConfig];

    public static readonly ImmutableArray<AnalyzerType> EditorConfig_GlobalConfig =
    [
        AnalyzerType.EditorConfig,
        AnalyzerType.GlobalConfig,
    ];

    public static readonly ImmutableArray<AnalyzerType> All =
    [
        AnalyzerType.INI,
        AnalyzerType.EditorConfig,
        AnalyzerType.GlobalConfig,
    ];
}
