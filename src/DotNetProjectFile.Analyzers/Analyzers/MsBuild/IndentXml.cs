namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class IndentXml : MsBuildProjectFileAnalyzer
{
    private readonly IdentationChecker<MsBuildProject> Checker;

    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    public IndentXml() : this(' ', 2) { }

    public IndentXml(char ch, int repeat) : base(Rule.IndentXml)
        => Checker = new(ch, repeat, Descriptor);

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
        => Checker.Walk(context.File, context.File.Text, context);
}
