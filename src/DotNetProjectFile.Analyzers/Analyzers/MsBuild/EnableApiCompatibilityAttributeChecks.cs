namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class EnableApiCompatibilityAttributeChecks()
    : MsBuildProjectFileAnalyzer(Rule.EnableApiCompatibilityAttributeChecks)
{
    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (!context.File.PackageValidationEnabled())
        {
            return;
        }

        if (context.File.Property<ApiCompatEnableRuleAttributesMustMatch>() is { } node && node.Value is not true)
        {
            context.ReportDiagnostic(Descriptor, node);
        }
        else
        {
            context.ReportDiagnostic(Descriptor, context.File);
        }
    }
}
