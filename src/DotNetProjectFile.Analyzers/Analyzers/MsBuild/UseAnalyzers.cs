namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseAnalyzers : MsBuildProjectFileAnalyzer
{
    public UseAnalyzers() : base(Rule.UseDotNetProjectFileAnalyzers) { }

    protected override void Register(ProjectFileAnalysisContext context)
    {
        var packageReferences = context.Project.AncestorsAndSelf()
            .SelectMany(p => p.ItemGroups)
            .SelectMany(group => group.PackageReferences)
            .ToArray();

        if (packageReferences.None(r => r.Include.Contains("DotNetProjectFile.Analyzers", StringComparison.OrdinalIgnoreCase)))
        {
            context.ReportDiagnostic(Descriptor, context.Project);
        }
    }
}
