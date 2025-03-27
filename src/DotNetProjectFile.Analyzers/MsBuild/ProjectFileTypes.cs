namespace DotNetProjectFile.MsBuild;

public static class ProjectFileTypes
{
    public static readonly IReadOnlyCollection<ProjectFileType> ProjectFile = [ProjectFileType.ProjectFile];

    public static readonly IReadOnlyCollection<ProjectFileType> SDK = [ProjectFileType.SDK];

    public static readonly IReadOnlyCollection<ProjectFileType> DirectoryPackages = [ProjectFileType.DirectoryPackages];

    public static readonly IReadOnlyCollection<ProjectFileType> ProjectFile_Props =
    [
        ProjectFileType.ProjectFile,
        ProjectFileType.Props
    ];

    public static readonly IReadOnlyCollection<ProjectFileType> ProjectFile_DirectoryBuild =
    [
        ProjectFileType.ProjectFile,
        ProjectFileType.DirectoryBuild
    ];

    public static readonly IReadOnlyCollection<ProjectFileType> ProjectFile_DirectoryPackages =
    [
        ProjectFileType.ProjectFile,
        ProjectFileType.DirectoryPackages
    ];

    public static readonly IReadOnlyCollection<ProjectFileType> AllExceptDirectoryPackages =
    [
        ProjectFileType.ProjectFile,
        ProjectFileType.Props,
        ProjectFileType.DirectoryBuild,
        ProjectFileType.SDK,
    ];

    public static readonly IReadOnlyCollection<ProjectFileType> AllExceptSDK =
    [
        ProjectFileType.ProjectFile,
        ProjectFileType.Props,
        ProjectFileType.DirectoryBuild,
        ProjectFileType.DirectoryPackages,
    ];

    public static readonly IReadOnlyCollection<ProjectFileType> All =
    [
        ProjectFileType.ProjectFile,
        ProjectFileType.Props,
        ProjectFileType.DirectoryBuild,
        ProjectFileType.DirectoryPackages,
        ProjectFileType.SDK,
    ];

    public static ProjectFileType GetProjectFileType(this IOFile file) => file switch
    {
        _ when file.Name.IsMatch(".net.csproj") => ProjectFileType.SDK,
        _ when file.Extension.IsMatch(".csproj")
            || file.Extension.IsMatch(".vbproj") => ProjectFileType.ProjectFile,
        _ when file.Name.IsMatch("Directory.Build.props")
            || file.Name.IsMatch("Directory.Build.targets") => ProjectFileType.DirectoryBuild,
        _ when file.Name.IsMatch("Directory.Packages.props") => ProjectFileType.DirectoryPackages,
        _ when file.Extension.IsMatch(".props")
            || file.Extension.IsMatch(".targets") => ProjectFileType.Props,
        _ => ProjectFileType.None,
    };
}
