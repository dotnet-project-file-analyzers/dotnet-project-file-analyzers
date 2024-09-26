namespace DotNetProjectFile.Analyzers.Resx;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefineData() : ResourceFileAnalyzer(Rule.DefineData)
{
    protected override void Register(ResourceFileAnalysisContext context)
    {
        if (context.File.Data.None())
        {
            context.ReportDiagnostic(Descriptor, context.File);
        }
    }
}
