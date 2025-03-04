namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.RemoveCommentedOutCode"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class RemoveCommentedOutCode() : MsBuildProjectFileAnalyzer(Rule.RemoveCommentedOutCode)
{
    private static readonly CommentChecker<MsBuildProject> Checker = new(Rule.RemoveCommentedOutCode, CommentChecker.ContainsXml);

    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
        => Checker.Check(context.File, context.File.Text, context);
}
