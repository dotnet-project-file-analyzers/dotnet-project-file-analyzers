namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Guards project reference compliance.</summary>>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class ProjectReferencesShouldBeCompliant() : MsBuildProjectFileAnalyzer(
    Rule.AvoidExecutableDependencies,
    Rule.DependendProjectsShouldBePackable,
    Rule.AvoidTestProjectDependencies)
{
    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext<MsBuildProject> context)
    {
        var root = context.File.Path.Directory;
        var info = ProjectReferenceInfo.Create(context.File);

        foreach (var reference in context.File.Walk()
            .OfType<ProjectReference>()
            .Where(r => r.Include is { Length: > 0 }))
        {
            if (context.File.ProjectFiles.MsBuildProject(root.File(reference.Include!)) is { } project)
            {
                var other = ProjectReferenceInfo.Create(project);
                var rule = info.ConflictsWith(other) switch
                {
                    ProjectReferenceConflict.IsExe => Rule.AvoidExecutableDependencies,
                    ProjectReferenceConflict.IsNotPackable => Rule.DependendProjectsShouldBePackable,
                    ProjectReferenceConflict.IsTestProject => Rule.AvoidTestProjectDependencies,
                    _ => null,
                };

                if (rule is { })
                {
                    context.ReportDiagnostic(
                        Descriptor,
                        reference,
                        reference.Include);
                }
            }
        }
    }
}
