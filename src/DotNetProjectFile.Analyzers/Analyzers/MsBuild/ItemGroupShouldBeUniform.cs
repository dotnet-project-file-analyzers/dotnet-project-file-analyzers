namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class ItemGroupShouldBeUniform : MsBuildProjectFileAnalyzer
{
    public ItemGroupShouldBeUniform() : base(Rule.ItemGroupShouldBeUniform) { }

    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var group in context.Project
            .ImportsAndSelf()
            .SelectMany(p => p.ItemGroups))
        {
            AnalyzeGroup(context, group);
        }
    }

    private void AnalyzeGroup(ProjectFileAnalysisContext context, ItemGroup group)
    {
        if (group.Children.FirstOrDefault()?.LocalName is not { } type)
        {
            return;
        }

        if (group.Children.Any(n => n.LocalName != type))
        {
            context.ReportDiagnostic(Descriptor, group);
        }
    }
}
