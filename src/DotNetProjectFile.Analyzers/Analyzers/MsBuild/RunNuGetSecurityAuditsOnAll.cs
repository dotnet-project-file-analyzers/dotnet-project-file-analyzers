namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.RunNuGetSecurityAuditsOnAll"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class RunNuGetSecurityAuditsOnAll() : MsBuildProjectFileAnalyzer(Rule.RunNuGetSecurityAuditsOnAll)
{
    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        var property = context.File.Property<NuGetAuditMode>();

        if (property?.Value is not NuGetAuditMode.Kind.All)
        {
            context.ReportDiagnostic(Descriptor, property as XmlAnalysisNode ?? context.File.Project);
        }
    }
}
