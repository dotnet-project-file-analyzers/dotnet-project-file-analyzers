using System;

namespace DotNetProjectFile.MsBuild;

public static class ProjectFileTypes
{
    /// <summary>Project files only.</summary>
    public static readonly ImmutableArray<ProjectFileType> ProjectFile = [ProjectFileType.ProjectFile];

    /// <summary>.net.csproj SDK only.</summary>
    public static readonly ImmutableArray<ProjectFileType> SDK = [ProjectFileType.SDK];

    /// <summary>Directory.Packages.props only.</summary>
    public static readonly ImmutableArray<ProjectFileType> DirectoryPackages = [ProjectFileType.DirectoryPackages];

    /// <summary>Project files and props/targets.</summary>
    public static readonly ImmutableArray<ProjectFileType> ProjectFile_Props =
    [
        ProjectFileType.ProjectFile,
        ProjectFileType.Props
    ];

    /// <summary>Project files and Directory.Build.props.</summary>
    public static readonly ImmutableArray<ProjectFileType> ProjectFile_DirectoryBuild =
    [
        ProjectFileType.ProjectFile,
        ProjectFileType.DirectoryBuild
    ];

    /// <summary>Project files and Directory.Packages.props.</summary>
    public static readonly ImmutableArray<ProjectFileType> ProjectFile_DirectoryPackages =
    [
        ProjectFileType.ProjectFile,
        ProjectFileType.DirectoryPackages
    ];

    /// <summary>All but Directory.Packages.props.</summary>
    public static readonly ImmutableArray<ProjectFileType> AllExceptDirectoryPackages =
    [
        ProjectFileType.ProjectFile,
        ProjectFileType.Props,
        ProjectFileType.DirectoryBuild,
        ProjectFileType.SDK,
    ];

    /// <summary>All but .net.csproj SDK.</summary>
    public static readonly ImmutableArray<ProjectFileType> AllExceptSDK =
    [
        ProjectFileType.ProjectFile,
        ProjectFileType.Props,
        ProjectFileType.DirectoryBuild,
        ProjectFileType.DirectoryPackages,
    ];

    /// <summary>All.</summary>
    public static readonly ImmutableArray<ProjectFileType> All =
    [
        ProjectFileType.ProjectFile,
        ProjectFileType.Props,
        ProjectFileType.DirectoryBuild,
        ProjectFileType.DirectoryPackages,
        ProjectFileType.SDK,
    ];

    /// <summary>Gets the <see cref="ProjectFileType"/> based on the file (name).</summary>
    public static ProjectFileType GetProjectFileType(this IOFile file) => file switch
    {
        _ when file.Name.IsMatch(".net.csproj") => ProjectFileType.SDK,
        _ when Language.All.Any(lang => file.Extension.IsMatch(lang.ProjectFile)) => ProjectFileType.ProjectFile,
        _ when file.Name.IsMatch("Directory.Build.props")
            || file.Name.IsMatch("Directory.Build.targets") => ProjectFileType.DirectoryBuild,
        _ when file.Name.IsMatch("Directory.Packages.props") => ProjectFileType.DirectoryPackages,
        _ when file.Extension.IsMatch(".props")
            || file.Extension.IsMatch(".targets") => ProjectFileType.Props,
        _ => ProjectFileType.None,
    };
}
