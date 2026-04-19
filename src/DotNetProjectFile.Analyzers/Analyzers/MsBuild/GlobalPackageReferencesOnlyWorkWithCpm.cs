namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.GlobalPackageReferencesOnlyWorkWithCpm"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class GlobalPackageReferencesOnlyWorkWithCpm()
    : MsBuildProjectFileAnalyzer(Rule.GlobalPackageReferencesOnlyWorkWithCpm)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.ManagePackageVersionsCentrally) return;

        foreach (var reference in context.File.ItemGroups.Children<GlobalPackageReference>())
        {
            context.ReportDiagnostic(Descriptor, reference, reference.Include);
        }
    }
}
