namespace DotNetProjectFile.Analyzers.MsBuild;

public sealed record ProjectReferenceInfo
{
    public IOFile Path { get; init; }

    public OutputType.Kind OutputType { get; init; }

    public bool IsPackable { get; init; } = false;

    public bool IsTestProject { get; init; } = false;

    [Pure]
    public ProjectReferenceConflict ConflictsWith(ProjectReferenceInfo dep) => dep switch
    {
        _ when dep.IsTestProject => ProjectReferenceConflict.IsTestProject,
        _ when IsPackable && !dep.IsPackable => ProjectReferenceConflict.IsNotPackable,
        _ when !IsTestProject && dep.OutputType.IsExe() => ProjectReferenceConflict.IsExe,
        _ => ProjectReferenceConflict.None,
    };

    [Pure]
    public static ProjectReferenceInfo Create(MsBuildProject project) => new()
    {
        Path = project.Path,
        OutputType = project.GetOutputType(),
        IsPackable = project.IsPackable(),
        IsTestProject = project.IsTestProject()
            || project.Walk().OfType<PackageReference>().Any(IsTestSdk),
    };

    [Pure]
    private static bool IsTestSdk(PackageReference reference) 
        => reference.Include is "Microsoft.NET.Test.Sdk";
}
