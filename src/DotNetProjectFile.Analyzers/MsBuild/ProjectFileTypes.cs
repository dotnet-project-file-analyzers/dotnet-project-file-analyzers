namespace DotNetProjectFile.MsBuild;

public static class ProjectFileTypes
{
    /// <summary>Project files only.</summary>
    public static readonly ImmutableArray<AnalyzerType> ProjectFile = [AnalyzerType.MSBuildProject];

    /// <summary>.net.csproj SDK only.</summary>
    public static readonly ImmutableArray<AnalyzerType> SDK = [AnalyzerType.SDK];

    /// <summary>Directory.Packages.props only.</summary>
    public static readonly ImmutableArray<AnalyzerType> DirectoryPackages = [AnalyzerType.DirectoryPackagesProps];

    /// <summary>Project files and props/targets.</summary>
    public static readonly ImmutableArray<AnalyzerType> ProjectFile_Props =
    [
        AnalyzerType.MSBuildProject,
        AnalyzerType.MSBuildProps,
    ];

    /// <summary>Project files and .net.csproj SDK.</summary>
    public static readonly ImmutableArray<AnalyzerType> ProjectFile_SDK =
    [
        AnalyzerType.MSBuildProject,
        AnalyzerType.SDK
    ];

    /// <summary>Project files and Directory.Packages.props.</summary>
    public static readonly ImmutableArray<AnalyzerType> ProjectFile_DirectoryPackages =
    [
        AnalyzerType.MSBuildProject,
         AnalyzerType.DirectoryBuildProps,
        AnalyzerType.DirectoryBuildTargets,
    ];

    /// <summary>All but Directory.Packages.props.</summary>
    public static readonly ImmutableArray<AnalyzerType> AllExceptDirectoryPackages =
    [
        AnalyzerType.MSBuildProject,
        AnalyzerType.MSBuildProps,
        AnalyzerType.DirectoryBuildProps,
        AnalyzerType.DirectoryBuildTargets,
        AnalyzerType.SDK,
    ];

    /// <summary>All but .net.csproj SDK.</summary>
    public static readonly ImmutableArray<AnalyzerType> AllExceptSDK =
    [
        AnalyzerType.MSBuildProject,
        AnalyzerType.MSBuildProps,
        AnalyzerType.DirectoryBuildProps,
        AnalyzerType.DirectoryBuildTargets,
        AnalyzerType.DirectoryPackagesProps,
    ];

    /// <summary>All.</summary>
    public static readonly ImmutableArray<AnalyzerType> All =
    [
        AnalyzerType.MSBuildProject,
        AnalyzerType.MSBuildProps,
        AnalyzerType.DirectoryBuildProps,
        AnalyzerType.DirectoryBuildTargets,
        AnalyzerType.DirectoryPackagesProps,
        AnalyzerType.SDK,
    ];
}
