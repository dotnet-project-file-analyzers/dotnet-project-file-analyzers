#pragma warning disable SA1118 // Parameter should not span multiple lines: readability for descriptions.

namespace DotNetProjectFile;

public static partial class Rule
{
    public static class Json
    {
        public static DiagnosticDescriptor Invalid => New(
           id: 6000,
           title: "Invalid JSON file",
           message: "{0}",
           description: "A part of the JSON file could not be parsed.",
           tags: ["JSON", "syntax error"],
           category: Category.SyntaxError);

        public static DiagnosticDescriptor GlobalJsonMustExist => New(
            id: 6010,
            title: "global.json should exist",
            message: "global.json does not exist",
            description:
                "To ensure a predictable .NET SDK version is used across " +
                "different machines and environments, a global.json file should " +
                "exist for every compiled project.",
            tags: ["global.json", "configuration", "SDK"],
            category: Category.CodeQuality);
    }
}
