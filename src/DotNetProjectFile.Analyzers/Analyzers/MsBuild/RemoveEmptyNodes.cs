namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class RemoveEmptyNodes() : MsBuildProjectFileAnalyzer(Rule.RemoveEmptyNodes)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var node in context.File.Children)
        {
            Report(node, context, 1);
        }
    }

    private void Report(Node node, ProjectFileAnalysisContext context, int level)
    {
        var noChildren = true;

        foreach (var child in node.Children)
        {
            noChildren = false;
            Report(child, context, level + 1);
        }

        if (noChildren && IsEmpty(node.Element, level))
        {
            context.ReportDiagnostic(Descriptor, node, node.LocalName);
        }
    }

    private static bool IsEmpty(XElement element, int level)
        => element.Name.LocalName != nameof(Import)
        && (level == 1
        || (element.Attributes().None() && string.IsNullOrWhiteSpace(element.Value)));
}
