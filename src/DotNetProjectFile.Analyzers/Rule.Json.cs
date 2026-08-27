#pragma warning disable SA1118 // Parameter should not span multiple lines: readability for descriptions.

namespace DotNetProjectFile;

public static partial class Rule
{
    internal static class Json
    {
        public static DiagnosticDescriptor Invalid => New(
           id: 6000,
           title: "Invalid JSON file",
           message: "{0}",
           description: "A part of the JSON file could not be parsed.",
           tags: ["JSON", "syntax error"],
           category: Category.SyntaxError);
    }
}
