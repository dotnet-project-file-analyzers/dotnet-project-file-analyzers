namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class EnablePackageValidation() : MsBuildProjectFileAnalyzer(Rule.EnablePackageValidation)
{
    protected override bool ApplyToProps => false;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (!PackageValidationEnabled(context.Project))
        {
            context.ReportDiagnostic(Descriptor, context.Project);
        }
    }

    private static bool PackageValidationEnabled(MsBuildProject project)
        => project.Property<bool?, DotNetProjectFile.MsBuild.EnablePackageValidation>(g => g.EnablePackageValidation, MsBuildDefaults.IsPackage).GetValueOrDefault();
}
