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

        public static DiagnosticDescriptor SpecifySdkRollForwardPolicy => New(
            id: 6012,
            title: "Specify SDK version roll-forward policy",
            message: "No valid SDK version roll-forward policy has been specified",
            description:
                "A global.json file should specify a valid value for the " +
                "sdk.rollForward property, so the .NET CLI behaves predictably " +
                "when the requested SDK version is not installed.",
            tags: ["global.json", "configuration", "SDK"],
            category: Category.CodeQuality);

        public static DiagnosticDescriptor DisableSdkRollForwardWhenLocked => New(
            id: 6013,
            title: "Disable SDK version roll-forward when using lock files",
            message: "Disable the SDK version roll-forward",
            description:
                "When a project uses lock files, the SDK version must be pinned " +
                "exactly by setting the sdk.rollForward property to 'disable'.",
            tags: ["global.json", "configuration", "SDK"],
            category: Category.CodeQuality);

        public static DiagnosticDescriptor SpecifyStableSdkVersion => New(
            id: 6014,
            title: "Specify stable SDK version",
            message: "Specify a stable SDK version",
            description: "The usage of pre-release .NET SDKs is strongly discouraged.",
            tags: ["global.json", "configuration", "SDK", "pre-release"],
            category: Category.CodeQuality);
    }
}
