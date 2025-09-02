namespace DotNetProjectFile.Analyzers.MsBuild;

/// <<summary>Implements <see cref="Rule.ProjectReferencesShouldBeCompliant"/>.</summary>>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class ProjectReferencesShouldBeCompliant() : MsBuildProjectFileAnalyzer(Rule.ProjectReferencesShouldBeCompliant)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

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
                var conflict = info.ConflictsWith(other);
                if (Message(conflict) is { } message)
                {
                    context.ReportDiagnostic(
                        Descriptor,
                        reference,
                        reference.Include,
                        message);
                }
            }
        }
    }

    public static string? Message(ProjectReferenceConflict conflict) => conflict switch
    {
        ProjectReferenceConflict.IsExe => "Do not depent on executables",
        ProjectReferenceConflict.IsTestProject => "Do not depent on test projects",
        ProjectReferenceConflict.IsNotPackable => "Remove the dependency or make it packable",
        _ => null,
    };
}
