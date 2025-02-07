namespace DotNetProjectFile.Analyzers.Resx;

/// <summary>Implements <see cref="Rule.AddAdditionalFile"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class AddAdditionalFile() : ResourceFileAnalyzer(Rule.AddAdditionalFile)
{
    /// <inheritdoc />
    protected override void Register(ResourceFileAnalysisContext context)
    {
        if (!context.File.IsAdditional(context.Options.AdditionalFiles))
        {
            context.ReportDiagnostic(Descriptor, context.File, ((ProjectFile)context.File).Path.Name);
        }
    }
}
