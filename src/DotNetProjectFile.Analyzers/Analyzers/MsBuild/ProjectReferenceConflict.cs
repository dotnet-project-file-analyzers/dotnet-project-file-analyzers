namespace DotNetProjectFile.Analyzers.MsBuild;

public enum ProjectReferenceConflict
{
    None,
    IsTestProject,
    IsNotPackable,
    IsExe,
}
