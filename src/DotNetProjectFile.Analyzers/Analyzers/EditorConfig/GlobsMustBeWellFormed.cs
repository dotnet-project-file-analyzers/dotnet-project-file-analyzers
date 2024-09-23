using DotNetProjectFile.Ini;

namespace DotNetProjectFile.Analyzers.EditorConfig;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class GlobsMustBeWellFormed() : EditorConfigFileAnalyzer(Rule.GlobsMustBeWellFormed)
{
    /// <inheritdoc/>
    protected override void Register(EditorConfigFileAnalysisContext context)
    {
        foreach (var header in context.File.Syntax.Sections.Select(s => s.Header).OfType<HeaderSyntax>())
        {
            if (Failure(header.Text) is string failure)
            {
                context.ReportDiagnostic(Descriptor, header.LinePositionSpan, header.Text, failure);
            }
        }
    }

    private static string? Failure(string header)
    {
        try
        {
            var matcher = new Microsoft.Extensions.FileSystemGlobbing.Matcher();
            matcher.AddInclude(header);
            return null;
        }
        catch (Exception x)
        {
            return x.Message;
        }
    }
}
