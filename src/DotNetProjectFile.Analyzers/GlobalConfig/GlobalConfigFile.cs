using DotNetProjectFile.Ini;

namespace DotNetProjectFile.GlobalConfig;

internal static class GlobalConfigFile
{
    extension(IniFile file)
    {
        public IEnumerable<AnalyzerDiagnosticSeverity> AnalyzerDiagnosticSeverities
            => file.Entries.Select(AnalyzerDiagnosticSeverity.Create).OfType<AnalyzerDiagnosticSeverity>();
    }
}
