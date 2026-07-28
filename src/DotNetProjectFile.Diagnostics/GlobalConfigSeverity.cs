namespace DotNetProjectFile.Diagnostics;

/// <summary>Severities used in the global configuration file.</summary>
public enum GlobalConfigSeverity
{
    /// <summary>None/not applicable.</summary>
    None = 0,

    /// <summary>Silent.</summary>
    Silent,

    /// <summary>Suggestion.</summary>
    Suggestion,

    /// <summary>Warning.</summary>
    Warning,

    /// <summary>Error.</summary>
    Error,
}
