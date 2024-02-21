namespace DotNetProjectFile.Diagnostics;

/// <summary>Config options used when no options were found. Always returns false for each lookup.</summary>
internal sealed class MissingAnalyzerConfigOptions : AnalyzerConfigOptions
{
    /// <summary>Gets a singleton instance.</summary>
    public static readonly AnalyzerConfigOptions Instance = new MissingAnalyzerConfigOptions();

    private MissingAnalyzerConfigOptions() { }

    /// <inheritdoc />
    public override bool TryGetValue(string key, [NotNullWhen(true)] out string? value)
    {
        value = null;
        return false;
    }
}
