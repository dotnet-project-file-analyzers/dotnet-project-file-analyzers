namespace DotNetProjectFile.Json;

public static class JsonFileTypes
{
    public static readonly ImmutableArray<AnalyzerType> Json = [AnalyzerType.Json];

    public static readonly ImmutableArray<AnalyzerType> GlobalJson = [AnalyzerType.GlobalJson];

    public static readonly ImmutableArray<AnalyzerType> All =
    [
        AnalyzerType.Json,
        AnalyzerType.GlobalJson,
    ];
}
