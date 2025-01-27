namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class EnableApiCompatibilityParameterNameChecks()
    : MsBuildProjectFileAnalyzer(Rule.EnableApiCompatibilityParameterNameChecks)
{
    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (!context.File.PackageValidationEnabled())
        {
            return;
        }

        if (context.File.Property<ApiCompatEnableRuleCannotChangeParameterName>() is not { } node)
        {
            context.ReportDiagnostic(Descriptor, context.File);
        }
        else if (node.Value is not true)
        {
            context.ReportDiagnostic(Descriptor, node);
        }
    }
}
