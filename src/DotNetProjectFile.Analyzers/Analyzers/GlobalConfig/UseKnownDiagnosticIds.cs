using DotNetProjectFile.GlobalConfig;
using DotNetProjectFile.Ini;
using System.Collections.Frozen;
using System.Reflection;

namespace DotNetProjectFile.Analyzers.GlobalConfig;

/// <summary>Implements <see cref="Rule.Ini.SpecifyIsGlobal"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseKnownDiagnosticIds()
    : IniFileAnalyzer(Rule.Ini.UseKnownDiagnosticIds)
{
    /// <inheritdoc />
    public override ImmutableArray<AnalyzerType> ApplicableTo => IniFileTypes.EditorConfig_GlobalConfig;

    /// <inheritdoc />
    protected override void Register(IniFileAnalysisContext context)
    {
        foreach (var entry in context.File.AnalyzerDiagnosticSeverities.Where(IsUknown))
        {
            context.ReportDiagnostic(Descriptor, context.File, entry.Key.LinePositionSpan);
        }
    }

    private bool IsUknown(AnalyzerDiagnosticSeverity entry)
        => entry.DiagnosticId.IsMatchStart("Proj")
        && !DiagnosticIds.Contains(entry.DiagnosticId);

    private readonly FrozenSet<string> DiagnosticIds = Init().ToFrozenSet(StringComparer.OrdinalIgnoreCase);

    private static HashSet<string> Init()
    {
        Type[] rules = [typeof(Rule), .. typeof(Rule).GetNestedTypes()];

        var ids = new HashSet<string>();

        foreach (var property in rules
            .SelectMany(t => t.GetProperties(BindingFlags.Static|BindingFlags.Public))
            .Where(p => p.PropertyType == typeof(DiagnosticDescriptor)))
        {
            ids.Add(((DiagnosticDescriptor)property.GetValue(null)).Id);
        }

        return ids;
    }
}
