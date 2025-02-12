namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.EnableApiCompatibilityAttributeChecks"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class EnableApiCompatibilityAttributeChecks()
    : MsBuildProjectFileAnalyzer(Rule.EnableApiCompatibilityAttributeChecks)
{
    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (!context.File.PackageValidationEnabled())
        {
            return;
        }

        if (context.File.Property<ApiCompatEnableRuleAttributesMustMatch>() is not { } node)
        {
            context.ReportDiagnostic(Descriptor, context.File);
        }
        else if (node.Value is not true)
        {
            context.ReportDiagnostic(Descriptor, node);
        }
    }
}
