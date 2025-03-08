namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class TestProjectShouldNotBePackable()
    : MsBuildProjectFileAnalyzer(Rule.TestProjectShouldNotBePackable)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.File.IsTestProject() && context.File.IsPackable())
        {
            var node = context.File.PropertyGroups.Children<IsPackable>()
                .OfType<XmlAnalysisNode>()
                .LastOrDefault() ?? context.File;

            context.ReportDiagnostic(Descriptor, node);
        }
    }
}
