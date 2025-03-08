namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefineUsingsExplicit() : MsBuildProjectFileAnalyzer(Rule.DefineUsingsExplicit)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var @implicit in context.File.PropertyGroups
            .Children<ImplicitUsings>(i => IsEnabled(i.Value)))
        {
            context.ReportDiagnostic(Descriptor, @implicit);
        }
    }

    private static bool IsEnabled(ImplicitUsings.Kind? implicitUsing)
        => implicitUsing == ImplicitUsings.Kind.@true
        || implicitUsing == ImplicitUsings.Kind.enable;
}
