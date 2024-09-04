using DotNetProjectFile.Xml;

namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class TestProjectShouldNotBePackable()
    : MsBuildProjectFileAnalyzer(Rule.TestProjectShouldNotBePackable)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.Project.IsTestProject() && context.Project.IsPackable())
        {
            var node = context.Project.PropertyGroups.SelectMany(p => p.IsPackable)
                .OfType<XmlAnalysisNode>()
                .LastOrDefault() ?? context.Project;

            context.ReportDiagnostic(Descriptor, node);
        }
    }
}
