namespace DotNetProjectFile.Analyzers.Resx;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefineData() : ResourceFileAnalyzer(Rule.DefineData)
{
    protected override void Register(ResourceFileAnalysisContext context)
    {
        if (context.Resource.Data.None())
        {
            context.ReportDiagnostic(Descriptor, context.Resource);
        }
    }
}
