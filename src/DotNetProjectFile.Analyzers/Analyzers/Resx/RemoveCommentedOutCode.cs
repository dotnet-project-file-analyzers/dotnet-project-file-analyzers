using DotNetProjectFile.Resx;

namespace DotNetProjectFile.Analyzers.Resx;

/// <<summary>Implements <see cref="Rule.RemoveCommentedOutCode"/>.</summary>>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class RemoveCommentedOutCode() : ResourceFileAnalyzer(Rule.RemoveCommentedOutCode)
{
    private static readonly CommentChecker<Resource> Checker = new(Rule.RemoveCommentedOutCode, CommentChecker.ContainsXml);

    /// <inheritdoc />
    protected override void Register(ResourceFileAnalysisContext context)
        => Checker.Check(context.File, context.File.Text, context);
}
