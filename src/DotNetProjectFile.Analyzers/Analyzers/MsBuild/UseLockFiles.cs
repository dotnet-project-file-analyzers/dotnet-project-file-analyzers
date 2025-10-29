namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseLockFiles() : MsBuildProjectFileAnalyzer(Rule.UseLockFiles)
{
    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        var project = context.File;
        if (project.Property<RestorePackagesWithLockFile>() is not { } node)
        {
            context.ReportDiagnostic(Descriptor, context.File);
        }
        else if (node.Value != true)
        {
            context.ReportDiagnostic(Descriptor, node);
        }
    }
}
