namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class EnableStrictModeForPackageRuntimeCompatibilityValidation()
    : MsBuildProjectFileAnalyzer(Rule.EnableStrictModeForPackageRuntimeCompatibilityValidation)
{
    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.File.PackageValidationEnabled()
         && context.File.Property<EnableStrictModeForCompatibleTfms>() is { } node && node.Value is not true)
        {
            context.ReportDiagnostic(Descriptor, node);
        }
    }
}
