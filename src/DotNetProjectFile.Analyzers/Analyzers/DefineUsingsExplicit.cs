namespace DotNetProjectFile.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefineUsingsExplicit : ProjectFileAnalyzer
{
    public DefineUsingsExplicit() : base(Rule.DefineUsingsExplicit) { }

    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var @implicit in context.Project.AncestorsAndSelf()
            .SelectMany(p => p.PropertyGroups)
            .SelectMany(g => g.ImplicitUsings)
            .Where(i => IsEnabled(i.Value)))
        {
            context.ReportDiagnostic(Descriptor, @implicit.Location);
        }
    }

    private bool IsEnabled(ImplicitUsings.Kind? implicitUsing)
        => implicitUsing == ImplicitUsings.Kind.@true
        || implicitUsing == ImplicitUsings.Kind.enable;
}
