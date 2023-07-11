namespace DotNetProjectFile.Analyzers.Resx;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class SortDataAlphabetically : ResourceFileAnalyzer
{
    public SortDataAlphabetically() : base(Rule.SortDataAlphabetically) { }

    protected override void Register(ResourceFileAnalysisContext context)
    {
        var sorted = context.Resource.Data
            .Select(d => d.Name)
            .OrderBy(d => d)
            .ToArray();

        for (var i = 0; i < sorted.Length; i++)
        {
            if (context.Resource.Data[i].Name != sorted[i])
            {
                context.ReportDiagnostic(Descriptor, context.Resource.Data[i]);
                return;
            }
        }
    }
}
