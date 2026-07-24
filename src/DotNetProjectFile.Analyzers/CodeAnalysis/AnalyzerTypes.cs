namespace DotNetProjectFile.CodeAnalysis;

internal static class AnalyzerTypes
{
    public static AnalyzerType? MsBuild(IOFile file) => file switch
    {
        _ when file.Name.IsMatch(".net.csproj") => AnalyzerType.SDK,
        _ when Languages.All.Any(lang => file.Extension.IsMatch(lang.ProjectFileExtension)) => AnalyzerType.MSBuildProject,
        _ when file.Name.IsMatch("Directory.Build.props") => AnalyzerType.DirectoryBuildTargets,
        _ when file.Name.IsMatch("Directory.Build.targets") => AnalyzerType.DirectoryBuildProps,
        _ when file.Name.IsMatch("Directory.Packages.props") => AnalyzerType.DirectoryPackagesProps,
        _ when file.Extension.IsMatch(".props")
            || file.Extension.IsMatch(".targets") => AnalyzerType.MSBuildProps,
        _ => null,
    };

    public static AnalyzerType? Ini(IOFile file) => file switch
    {
        _ when file.Name.IsMatch(".editorconfig") => AnalyzerType.EditorConfig,
        _ when file.Name.IsMatch(".globalconfig") => AnalyzerType.GlobalConfig,
        _ when file.Extension.IsMatch(".ini") => AnalyzerType.INI,
        _ => null,
    };
}
