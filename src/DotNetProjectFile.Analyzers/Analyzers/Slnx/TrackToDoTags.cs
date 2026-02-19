using DotNetProjectFile.Slnx;

namespace DotNetProjectFile.Analyzers.Slnx;

/// <summary>Implements <see cref="Rule.TrackToDoTags"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class TrackToDoTags() : SolutionFileAnalyzer(Rule.TrackToDoTags)
{
    private readonly ToDoChecker<SolutionFile> Checker = new(Rule.TrackToDoTags, _ => null);

    /// <inheritdoc />
    protected override void Register(SolutionFileAnalysisContext context)
        => Checker.Check(context.File, context.File.Text, context);
}
