namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class AdoptPreferredCasing() : MsBuildProjectFileAnalyzer(Rule.AdoptPreferredCasing)
{
    protected override void Register(ProjectFileAnalysisContext context) => Walk(context.File, context);

    private void Walk(Node node, ProjectFileAnalysisContext context)
    {
        if (node is not Unknown && node.LocalName != node.Element.Name.LocalName)
        {
            context.ReportDiagnostic(Descriptor, node, node.Element.Name.LocalName, node.LocalName);
        }

        foreach (var child in node.Children)
        {
            Walk(child, context);
        }
    }
}
