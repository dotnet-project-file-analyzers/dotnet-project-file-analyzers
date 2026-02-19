namespace DotNetProjectFile.Analyzers.Helpers;

/// <summary>Implements <see cref="Rule.AddAdditionalFile"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class CompilerVisibleProperties() : MsBuildProjectFileAnalyzer(Rule.CompilerVisibleProperties)
{
    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        var options = context.Options;

        Report("Configuration");
        Report("LangVersion");
        Report("MSBuildProjectFile");
        Report("MSBuildThisFileDirectory");
        Report("NETCoreSdkVersion");
        Report("PackageLicenseExpression");
        Report("Platform");
        Report("SolutionDir");

        void Report(string property)
        {
            var value = options.GetMsBuildProperty(property);
            context.ReportDiagnostic(Descriptor, context.File, property, value ?? "{null}");
        }
    }
}
