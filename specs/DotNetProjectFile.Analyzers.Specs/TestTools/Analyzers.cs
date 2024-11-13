namespace Specs.TestTools;

internal static class AvailableAnalyzers
{
    public static IEnumerable<MsBuildProjectFileAnalyzer> MS_Build
        => All.OfType<MsBuildProjectFileAnalyzer>();

    [Pure]
    public static IEnumerable<DiagnosticAnalyzer> All => typeof(MsBuildProjectFileAnalyzer).Assembly
        .GetTypes()
        .Where(t => !t.IsAbstract && t.IsAssignableTo(typeof(DiagnosticAnalyzer)))
        .Select(t => (DiagnosticAnalyzer)Activator.CreateInstance(t)!)
        .Where(a => a.SupportedDiagnostics[0].IsEnabledByDefault);
}
