namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.DefineIsPackable"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefineIsPackable() : MsBuildProjectFileAnalyzer(Rule.DefineIsPackable)
{
    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (!context.File.IsTestProject() &&
            context.File.Property<IsPackable>() is null)
        {
            context.ReportDiagnostic(Descriptor, context.File);
        }
    }
}
