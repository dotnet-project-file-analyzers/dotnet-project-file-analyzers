using DotNetProjectFile.NuGet.Configuration;

namespace DotNetProjectFile.Analyzers.NuGetConfig;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class IndentXml : NuGetConfigFileAnalyzer
{
    private readonly IdentationChecker<NuGetConfigFile> Checker;

    public IndentXml() : this(' ', 2) { }

    public IndentXml(char ch, int repeat) : base(Rule.IndentXml)
        => Checker = new(ch, repeat, Descriptor);

    /// <inheritdoc />
    protected override void Register(NuGetConfigFileAnalysisContext context)
        => Checker.Walk(context.File, context.File.Text, context);
}
