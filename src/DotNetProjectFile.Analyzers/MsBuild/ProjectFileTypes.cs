namespace DotNetProjectFile.MsBuild;

public static class ProjectFileTypes
{
    public static readonly IReadOnlyCollection<ProjectFileType> ProjectFile = [ProjectFileType.ProjectFile];

    public static readonly IReadOnlyCollection<ProjectFileType> ProjectFileAndProps =
    [
        ProjectFileType.ProjectFile,
        ProjectFileType.Props
    ];

    public static readonly IReadOnlyCollection<ProjectFileType> All =
    [
        ProjectFileType.ProjectFile,
        ProjectFileType.Props,
        ProjectFileType.DirectoryBuild,
        ProjectFileType.DirectoryPackages,
    ];
}
