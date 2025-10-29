namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class EnableStrictModeForPackageFrameworkCompatibilityValidation()
    : MsBuildProjectFileAnalyzer(Rule.EnableStrictModeForPackageFrameworkCompatibilityValidation)
{
    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (!context.File.PackageValidationEnabled())
        {
            return;
        }

        if (context.File.Property<EnableStrictModeForCompatibleFrameworksInPackage>() is not { } node)
        {
            context.ReportDiagnostic(Descriptor, context.File);
        }
        else if (node.Value is not true)
        {
            context.ReportDiagnostic(Descriptor, node);
        }
    }
}
