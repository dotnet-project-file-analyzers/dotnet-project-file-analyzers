namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class IndentXml : MsBuildProjectFileAnalyzer
{
    private readonly IdentationChecker<ProjectFileAnalysisContext> Checker;

    public IndentXml() : this(' ', 2) { }

    public IndentXml(char ch, int repeat) : base(Rule.IndentXml)
        => Checker = new(ch, repeat, Description);

    protected override void Register(ProjectFileAnalysisContext context)
        => Checker.Walk(context.Project, context.Project.Text, context);
}
