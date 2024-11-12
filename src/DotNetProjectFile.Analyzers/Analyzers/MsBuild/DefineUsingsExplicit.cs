namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefineUsingsExplicit() : MsBuildProjectFileAnalyzer(Rule.DefineUsingsExplicit)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var @implicit in context.File.PropertyGroups
            .SelectMany(g => g.ImplicitUsings)
            .Where(i => IsEnabled(i.Value)))
        {
            context.ReportDiagnostic(Descriptor, @implicit);
        }
    }

    private static bool IsEnabled(ImplicitUsings.Kind? implicitUsing)
        => implicitUsing == ImplicitUsings.Kind.@true
        || implicitUsing == ImplicitUsings.Kind.enable;
}
