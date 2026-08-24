using DotNetProjectFile.Ini;

namespace DotNetProjectFile.Analyzers.Ini;

/// <summary>Implements <see cref="Rule.Ini.DefineKeysOnce"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefineKeysOnce() : IniFileAnalyzer(Rule.Ini.DefineKeysOnce)
{
    /// <inheritdoc />
    protected override void Register(IniFileAnalysisContext context)
    {
        var keys = new Dictionary<string, IniKey>(StringComparer.OrdinalIgnoreCase);

        foreach (var section in context.File.Sections)
        {
            keys.Clear();

            foreach (var entry in section.Entries)
            {
                if (entry.Key is not { Text.Length: > 0 } key) continue;

                if (keys.TryGetValue(key.Text, out var existing))
                {
                    context.ReportDiagnostic(Descriptor, context.File, key.LinePositionSpan, key.Text, existing.LinePositionSpan.Start.Line + 1);
                }
                else
                {
                    keys[key.Text] = key;
                }
            }
        }
    }
}
