namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.PreferAttributesOverElements"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class PreferAttributesOverElements() : MsBuildProjectFileAnalyzer(Rule.PreferAttributesOverElements)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context) => Walk(context.File, context);

    private void Walk(Node node, ProjectFileAnalysisContext context)
    {
        if (node.Children.Any())
        {
            foreach (var child in node.Children.Where(Applicable))
                Walk(child, context);
        }
        else if (CanBeAttribute(node))
        {
            context.ReportDiagnostic(Descriptor, node, node.LocalName);
        }
    }

    private static bool Applicable(Node node)
        => node is not PropertyGroup;

    private static bool CanBeAttribute(Node node)
        => node.Element.Attributes().None()
        && !node.Element.Value.Trim().Contains('\n');
}
