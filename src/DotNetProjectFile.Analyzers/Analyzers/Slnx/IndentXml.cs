using DotNetProjectFile.Slnx;

namespace DotNetProjectFile.Analyzers.Slnx;

/// <summary>Implements <see cref="Rule.IndentXml"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class IndentXml : SolutionFileAnalyzer
{
    public IndentXml(char ch, int repeat) : base(Rule.IndentXml) => Checker = new(ch, repeat, Descriptor);

    public IndentXml() : this(' ', 2) { }

    private readonly IdentationChecker<Solution> Checker;

    /// <inheritdoc />
    protected override void Register(SolutionFileAnalysisContext context)
        => Checker.Walk(context.File, context.File.Text, context);
}
