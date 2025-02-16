namespace Specs.TestTools;

internal static class AdHoc
{
    public static DiagnosticDescriptor Rule(string message, string id = "AdHoc01") =>
        new(
            id: id,
            title: message,
            messageFormat: message,
            category: "AdHoc",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true);
}
