namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class EnablePackageValidation() : MsBuildProjectFileAnalyzer(Rule.EnablePackageValidation)
{
    /// <inheritdoc />
    public override ImmutableArray<AnalyzerType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        var project = context.File;
        if (project.IsPackable() && !context.IsDevelopmentDependency && !project.PackageValidationEnabled())
        {
            context.ReportDiagnostic(Descriptor, context.File);
        }
    }
}
