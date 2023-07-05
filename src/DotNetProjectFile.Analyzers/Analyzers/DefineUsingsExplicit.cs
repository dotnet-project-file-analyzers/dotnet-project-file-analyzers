namespace DotNetProjectFile.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefineUsingsExplicit : ProjectFileAnalyzer
{
    public DefineUsingsExplicit() : base(Rule.DefineUsingsExplicit) { }

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.Project.GetSelfAndAncestors()
            .SelectMany(p => p.PropertyGroups)
            .SelectMany(g => g.ImplicitUsings)
            .LastOrDefault(i => IsEnabled(i.Value)) is { } node)
        {
            context.ReportDiagnostic(Descriptor, node.Location);
        }
    }

    private bool IsEnabled(ImplicitUsings.Kind? implicitUsing)
        => implicitUsing == ImplicitUsings.Kind.@true
        || implicitUsing == ImplicitUsings.Kind.enable;
}
