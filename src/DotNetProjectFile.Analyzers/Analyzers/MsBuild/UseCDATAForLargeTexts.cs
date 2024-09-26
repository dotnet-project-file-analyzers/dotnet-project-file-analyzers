namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseCDATAForLargeTexts() : MsBuildProjectFileAnalyzer(Rule.UseCDATAForLargeTexts)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        Walk(context.File);

        void Walk(Node node)
        {
            if (node is PackageReleaseNotes && !node.Element.ContainsCDATA())
            {
                context.ReportDiagnostic(Descriptor, node.Positions.InnerSpan);
            }

            foreach (var child in node.Children)
            {
                Walk(child);
            }
        }
    }
}
