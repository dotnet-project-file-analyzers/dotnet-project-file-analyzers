
namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class NewRule() : MsBuildProjectFileAnalyzer(Rule.DefineIsPublishable)
{
    protected override void Register(ProjectFileAnalysisContext<MsBuildProject> context)
    {
        Walk(context.File, context);
    }

    private void Walk(Node node, ProjectFileAnalysisContext<MsBuildProject> context)
    {
        if (node is Unknown)
        {
            context.ReportDiagnostic(Descriptor, node);
        }

        foreach (var child in node.Children)
        {
            Walk(child, context);
        }
    }
}
