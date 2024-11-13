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
}
