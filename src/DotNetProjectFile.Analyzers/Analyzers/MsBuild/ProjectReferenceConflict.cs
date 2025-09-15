namespace DotNetProjectFile.Analyzers.MsBuild;

public enum ProjectReferenceConflict
{
    None = 0,
    IsExe,
    IsNotPackable,
    IsTestProject,
}
