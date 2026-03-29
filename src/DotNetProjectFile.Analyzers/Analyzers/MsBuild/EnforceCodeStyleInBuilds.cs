namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.EnforceCodeStyleInBuilds"/>..</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class EnforceCodeStyleInBuilds() : MsBuildProjectFileAnalyzer(Rule.EnforceCodeStyleInBuilds)
{
    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.File.Property<EnforceCodeStyleInBuild>() is null)
        {
            context.ReportDiagnostic(Descriptor, context.File);
        }
    }
}
