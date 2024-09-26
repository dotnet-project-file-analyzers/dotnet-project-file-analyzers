namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefinePackageValidationBaselineVersion() : MsBuildProjectFileAnalyzer(Rule.DefinePackageValidationBaselineVersion)
{
    protected override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.File.PackageValidationEnabled() &&
            context.File.Property<PackageValidationBaselineVersion>() is null)
        {
            context.ReportDiagnostic(Descriptor, context.File);
        }
    }
}
