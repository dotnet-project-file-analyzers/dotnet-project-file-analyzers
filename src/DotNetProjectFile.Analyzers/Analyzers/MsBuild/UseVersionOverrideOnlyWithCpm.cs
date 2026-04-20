namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.UseVersionOverrideOnlyWithCpm"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseVersionOverrideOnlyWithCpm()
    : MsBuildProjectFileAnalyzer(Rule.UseVersionOverrideOnlyWithCpm)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.ManagePackageVersionsCentrally) return;

        foreach (var reference in context.File.ItemGroups
            .Children<PackageReference>(r => r.VersionOverride is { Length: > 0 }))
        {
            context.ReportDiagnostic(Descriptor, reference);
        }
    }
}
