using DotNetProjectFile.Resx;

namespace DotNetProjectFile.Analyzers.Resx;

/// <summary>Implements <see cref="Rule.RESX.Indent"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class IndentResx : ResourceFileAnalyzer
{
    private readonly IdentationChecker<Resource> Checker;

    public IndentResx() : this(' ', 2) { }

    public IndentResx(char ch, int repeat) : base(Rule.RESX.Indent)
        => Checker = new(ch, repeat, Descriptor, Exclude);

    /// <inheritdoc />
    protected override void Register(ResourceFileAnalysisContext context)
        => Checker.Walk(context.File, context.File.Text, context);

    /// <remarks>
    /// Excludes children of value.
    /// </remarks>
    private static bool Exclude(XmlAnalysisNode node)
        => node.Element.Ancestors().Any(n => n.Name.LocalName == "value");
}
