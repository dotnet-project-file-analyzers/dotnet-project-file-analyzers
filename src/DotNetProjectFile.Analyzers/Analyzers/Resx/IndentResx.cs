namespace DotNetProjectFile.Analyzers.Resx;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class IndentResx : ResourceFileAnalyzer
{
    private readonly IdentationChecker<ResourceFileAnalysisContext> Checker;

    public IndentResx() : this(' ', 2) { }

    public IndentResx(char ch, int repeat) : base(Rule.IndentResx)
        => Checker = new(ch, repeat, Descriptor);

    protected override void Register(ResourceFileAnalysisContext context)
        => Checker.Walk(context.Resource, context.Resource.Text, context);
}
