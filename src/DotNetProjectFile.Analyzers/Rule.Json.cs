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

        public static DiagnosticDescriptor SpecifySdkVersion => New(
            id: 6011,
            title: "Specify SDK version",
            message: "No valid SDK version has been specified",
            description:
                "A global.json file should specify a valid .NET SDK version in " +
                "its sdk.version property, so the SDK resolver can pin the SDK " +
                "to a predictable release across machines and environments.",
            tags: ["global.json", "configuration", "SDK"],
            category: Category.CodeQuality);
    }
}
