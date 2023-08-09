namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class AvoidUsingMoq : MsBuildProjectFileAnalyzer
{
    public AvoidUsingMoq() : base(Rule.AvoidUsingMoq) { }

    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var reference in context.Project.ItemGroups
            .SelectMany(i => i.PackageReferences)
            .Where(p => p.Include == "Moq"))
        {
            context.ReportDiagnostic(Descriptor, reference);
        }
    }
}
