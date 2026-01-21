namespace DotNetProjectFile.Analyzers.NuGetConfig;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class ClearPreviousPackageSources() : NuGetConfigFileAnalyzer(Rule.AddAdditionalFile)
{
    protected override void Register(NuGetConfigFileAnalysisContext context)
    {
    }
}
