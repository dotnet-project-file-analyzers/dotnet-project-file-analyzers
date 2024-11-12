using DotNetProjectFile.Resx;

namespace DotNetProjectFile.Analyzers.Resx;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class IndentResx : ResourceFileAnalyzer
{
    private readonly IdentationChecker<Resource> Checker;

    public IndentResx() : this(' ', 2) { }

    public IndentResx(char ch, int repeat) : base(Rule.IndentResx)
        => Checker = new(ch, repeat, Descriptor);

    protected override void Register(ResourceFileAnalysisContext context)
        => Checker.Walk(context.File, context.File.Text, context, Exclude);

    /// <remarks>
    /// Excludes children of value.
    /// </remarks>
    private static bool Exclude(XmlAnalysisNode node)
        => node.Element.Ancestors().Any(n => n.Name.LocalName == "value");
}
