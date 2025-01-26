namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class EnableStrictModeForPackageBaselineValidation()
    : MsBuildProjectFileAnalyzer(Rule.EnableStrictModeForPackageBaselineValidation)
{
    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (!context.File.PackageValidationEnabled()
         || context.File.Property<PackageValidationBaselineVersion>() is null)
        {
            return;
        }

        if (context.File.Property<EnableStrictModeForBaselineValidation>() is { } node && node.Value is not true)
        {
            context.ReportDiagnostic(Descriptor, node);
        }
        else
        {
            context.ReportDiagnostic(Descriptor, context.File);
        }
    }
}
