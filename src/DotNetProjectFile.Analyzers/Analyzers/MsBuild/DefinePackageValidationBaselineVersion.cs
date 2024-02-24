namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefinePackageValidationBaselineVersion() : MsBuildProjectFileAnalyzer(Rule.DefinePackageValidationBaselineVersion)
{
    protected override bool ApplyToProps => false;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (!PackageValidationEnabled(context.Project))
        {
            return;
        }

        if (context.Project
            .ImportsAndSelf()
            .SelectMany(p => p.PropertyGroups)
            .SelectMany(g => g.PackageValidationBaselineVersion).None())
        {
            context.ReportDiagnostic(Descriptor, context.Project);
        }
    }

    private static bool PackageValidationEnabled(MsBuildProject project)
        => project.Property<bool?, DotNetProjectFile.MsBuild.EnablePackageValidation>(g => g.EnablePackageValidation, MsBuildDefaults.IsPackage).GetValueOrDefault();
}
