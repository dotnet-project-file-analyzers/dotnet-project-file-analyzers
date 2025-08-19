namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.PublishExeOnly"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class PublishExeOnly() : MsBuildProjectFileAnalyzer(Rule.PublishExeOnly)
{
    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        var project = context.File.Project;

        if (!project.GetOutputType().IsExe()
            && project.Property<IsPublishable>() is { } node
            && node.Value is true)
        {
            context.ReportDiagnostic(Descriptor, node);
        }
    }
}
