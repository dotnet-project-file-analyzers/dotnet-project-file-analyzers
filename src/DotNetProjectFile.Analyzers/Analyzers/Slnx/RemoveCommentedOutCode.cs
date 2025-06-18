using DotNetProjectFile.Slnx;

namespace DotNetProjectFile.Analyzers.Slnx;

/// <summary>Implements <see cref="Rule.RemoveCommentedOutCode"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class RemoveCommentedOutCode() : SolutionFileAnalyzer(Rule.RemoveCommentedOutCode)
{
    private static readonly CommentChecker<Solution> Checker = new(Rule.RemoveCommentedOutCode, CommentChecker.ContainsXml);

    /// <inheritdoc />
    protected override void Register(SolutionFileAnalysisContext context)
        => Checker.Check(context.File, context.File.Text, context);
}
