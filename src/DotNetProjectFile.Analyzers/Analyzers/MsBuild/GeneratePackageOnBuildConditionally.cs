namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class GeneratePackageOnBuildConditionally()
    : MsBuildProjectFileAnalyzer(Rule.GeneratePackageOnBuildConditionally)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var generate in context.File.PropertyGroups
           .Children<GeneratePackageOnBuild>(Unconditional))
        {
            context.ReportDiagnostic(Descriptor, generate);
        }
    }

    private static bool Unconditional(GeneratePackageOnBuild generate)
        => generate.AncestorsAndSelf().All(n => n.Condition is not { Length: > 0 });
}
