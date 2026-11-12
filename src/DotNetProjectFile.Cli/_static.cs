namespace DotNetProjectFile.Cli;

internal static class Statics
{
    /// <summary>The non-breaking space, to keep a number and its unit together.</summary>
    public const string Nbsp = "\u00A0";

    public static string invariant(FormattableString str) => FormattableString.Invariant(str);
}
