namespace Microsoft.CodeAnalysis.Diagnostics;

internal static class AnalyzerOptionsExtensions
{
    /// <summary>Gets the MS Build property.</summary>
    [Pure]
    public static string? GetMsBuildProperty(this AnalyzerOptions options, string propertyName)
        => options.AnalyzerConfigOptionsProvider.GlobalOptions
            .TryGetValue($"build_property.{propertyName}", out var value)
            ? value
            : null;
}
