using DotNetProjectFile.Slnx;

namespace DotNetProjectFile.Analyzers.Slnx;

/// <summary>Implements <see cref="Rule.RemoveCommentedOutCode"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class FileShouldExist() : SolutionFileAnalyzer(Rule.SLNX.FileShouldExist)
{
    /// <inheritdoc />
    protected override void Register(SolutionFileAnalysisContext context)
    {
        foreach (var node in context.File
            .DescendantsAndSelf()
            .OfType<File>()
            .Where(n => n.FullPath is { Exists: false, Info: not null }))
        {
            context.ReportDiagnostic(Descriptor, node, node.Path);
        }
    }
}
