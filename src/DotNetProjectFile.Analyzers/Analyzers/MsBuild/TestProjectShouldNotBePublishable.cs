namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class TestProjectShouldNotBePublishable()
    : MsBuildProjectFileAnalyzer(Rule.TestProjectShouldNotBePublishable)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.Project.IsTestProject() && context.Project.IsPublishable())
        {
            var node = context.Project.PropertyGroups.SelectMany(p => p.IsPublishable)
                .OfType<XmlAnalysisNode>()
                .LastOrDefault() ?? context.Project;

            context.ReportDiagnostic(Descriptor, node);
        }
    }
}
