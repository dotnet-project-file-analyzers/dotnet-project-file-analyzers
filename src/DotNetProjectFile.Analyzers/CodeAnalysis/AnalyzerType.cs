namespace DotNetProjectFile.CodeAnalysis;

/// <summary>
/// <![CDATA[
/// The type as set to files marked as <AdditionalFiles AnalyzerType="{type}">
/// by the analyzers.
/// ]]>
/// </summary>
public enum AnalyzerType
{
    /// <summary>None/not set/unparsable (default).</summary>
    None = 0,

    // MSBuild
    MSBuildProject,
    MSBuildProps,
    DirectoryBuildProps,
    DirectoryBuildTargets,
    DirectoryPackagesProps,
    SDK,

    // INI
    EditorConfig,
    GlobalConfig,
    INI,

    NuGetConfig,

    RESX,

    SLNX,
}
