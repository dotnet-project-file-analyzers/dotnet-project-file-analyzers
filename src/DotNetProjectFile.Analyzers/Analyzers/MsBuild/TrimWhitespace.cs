namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.TrimWhitespace"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class TrimWhitespace() : MsBuildProjectFileAnalyzer(Rule.TrimWhitespace)
{
    private readonly WhitespaceChecker<MsBuildProject> Checker = new(Rule.TrimWhitespace);

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
        => Checker.Check(context.File, context.File.Text, context);
}
