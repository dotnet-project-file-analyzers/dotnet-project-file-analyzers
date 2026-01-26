using DotNetProjectFile.NuGet.Configuration;

namespace DotNetProjectFile.Analyzers.NuGetConfig;

/// <summary>Implements <see cref="Rule.TrimWhitespace"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class TrimWhitespace() : NuGetConfigFileAnalyzer(Rule.TrimWhitespace)
{
    private readonly WhitespaceChecker<NuGetConfigFile> Checker = new(Rule.TrimWhitespace);

    /// <inheritdoc />
    protected override void Register(NuGetConfigFileAnalysisContext context)
        => Checker.Check(context.File, context.File.Text, context);
}
