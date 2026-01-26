namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.NuGetSecurityAuditShouldReportModerateAndUp"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class NuGetSecurityAuditShouldReportModerateAndUp() : MsBuildProjectFileAnalyzer(Rule.NuGetSecurityAuditShouldReportModerateAndUp)
{
    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        var property = context.File.Property<NuGetAuditLevel>();
        var level = property?.Value ?? NuGetAuditLevel.Kind.Low;

        if (level > NuGetAuditLevel.Kind.Moderate)
        {
            context.ReportDiagnostic(Descriptor, property as XmlAnalysisNode ?? context.File.Project);
        }
    }
}
