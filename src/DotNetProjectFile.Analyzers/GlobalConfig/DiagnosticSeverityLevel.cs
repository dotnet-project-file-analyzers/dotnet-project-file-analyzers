namespace DotNetProjectFile.GlobalConfig;

public enum DiagnosticSeverityLevel
{

    /// <summary>
    /// Suppresses the diagnostic completely; it does not run, nor does it offer fixes.
    /// </summary>
    none = 1,

    /// <summary>
    /// The default severity of the rule is used.
    /// </summary>
    @default,

    /// <summary>
    /// The diagnostic is invisible to the user in the UI, but still triggers
    /// the IDE to offer associated light-bulb code fixes.
    /// </summary>
    silent,

    /// <summary>
    /// olations appear as messages/gray dots in the Error List and do not fail
    /// the build.
    /// </summary>
    suggestion,

    /// <summary>
    /// Violations appear in the Error List as a green squiggle but do not
    /// fail the build.
    /// </summary>
    warning,

    /// <summary>
    /// Violations appear in the Error List, cause command-line builds to fail,
    /// and are marked with a red squiggle.
    /// </summary>
    error,
}
