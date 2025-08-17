namespace DotNetProjectFile.Analyzers.Resx;

/// <summary>Implements <see cref="Rule.RESX.DefineData"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefineData() : ResourceFileAnalyzer(Rule.RESX.DefineData)
{
    /// <inheritdoc />
    protected override void Register(ResourceFileAnalysisContext context)
    {
        if (context.File.Data.None())
        {
            context.ReportDiagnostic(Descriptor, context.File);
        }
    }
}
