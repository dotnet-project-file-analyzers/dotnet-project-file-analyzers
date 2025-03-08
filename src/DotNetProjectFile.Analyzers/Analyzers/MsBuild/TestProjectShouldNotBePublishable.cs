namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class TestProjectShouldNotBePublishable()
    : MsBuildProjectFileAnalyzer(Rule.TestProjectShouldNotBePublishable)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.File.IsTestProject() && context.File.IsPublishable())
        {
            var node = context.File.PropertyGroups.Children<IsPublishable>()
                .OfType<XmlAnalysisNode>()
                .LastOrDefault() ?? context.File;

            context.ReportDiagnostic(Descriptor, node);
        }
    }
}
