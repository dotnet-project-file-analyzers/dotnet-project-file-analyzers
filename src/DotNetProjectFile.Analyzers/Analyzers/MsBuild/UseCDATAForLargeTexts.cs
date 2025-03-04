namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.UseCDATAForLargeTexts"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseCDATAForLargeTexts() : MsBuildProjectFileAnalyzer(Rule.UseCDATAForLargeTexts)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        Walk(context.File);

        void Walk(Node node)
        {
            if (node is PackageReleaseNotes &&
                LineCount(node) > 3 &&
                !node.Element.ContainsCDATA())
            {
                context.ReportDiagnostic(Descriptor, node.Locations.InnerSpan);
            }

            foreach (var child in node.Children)
            {
                Walk(child);
            }
        }
    }

    [Pure]
    private static int LineCount(Node node)
        => 1
        + node.Locations.Positions.FullSpan.End.Line
        - node.Locations.Positions.FullSpan.Start.Line;
}
