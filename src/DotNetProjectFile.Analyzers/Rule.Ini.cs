#pragma warning disable SA1118 // Parameter should not span multiple lines: readability for descriptions.

namespace DotNetProjectFile;

public static partial class Rule
{
    public static class Ini
    {
        public static DiagnosticDescriptor Invalid => New(
           id: 4000,
           title: "Invalid INI file",
           message: "{0}",
           description: "A part of the INI file could not be parsed.",
           tags: ["INI", "syntax error"],
           category: Category.SyntaxError);

        public static DiagnosticDescriptor InvalidHeader => New(
           id: 4001,
           title: "Invalid INI Header",
           message: "{0}",
           description: "An INI header should have the format [<Header>].",
           tags: ["INI", "syntax error"],
           category: Category.SyntaxError);

        public static DiagnosticDescriptor InvalidKeyValuePair => New(
           id: 4002,
           title: "Invalid INI key-value pair",
           message: "{0}",
           description: "An INI key-value pair should have the format <Key> ( : | = ) <Value>.",
           tags: ["INI", "syntax error"],
           category: Category.SyntaxError);

        public static DiagnosticDescriptor EmptySection => New(
            id: 4010,
            title: "Sections should contain at least one key-value pair",
            message: "Section [{0}] is empty",
            description:
                "A Section in INI file groups key-value pairs. Having an empty " +
                "section has no added value.",
            tags: ["INI", "noise"],
            category: Category.Noise);

        public static DiagnosticDescriptor RemoveSectionHeader => New(
            id: 4025,
            title: "Remove section header",
            message: "Remove section header",
            description:
                "A .globalconfig file strictly requires all key-value to sit at " +
                "the root level; any section headers or glob patterns inside " +
                "the file are ignored by the Roslyn compiler.",
            tags: [".globalconfig", "header"],
            category: Category.CodeSmell);

        public static DiagnosticDescriptor SpecifyIsGlobal => New(
            id: 4026,
            title: "Specify is_global",
            message: "{0} is_global",
            description:
                "By explicitly setting is_global = true, the settings are correctly " +
                "applied globally across the entire project or solution, even if " +
                "the file uses a custom name instead of .globalconfig.",
            tags: [".globalconfig", "header"],
            category: Category.Clarity);

        public static DiagnosticDescriptor UseValidSeverityLevel => New(
            id: 4027,
            title: "Use valid diagnostic severity value",
            message: "diagnostic severity '{0}' is unknown",
            description:
                "The compiler requires standard severity keywords to parse .globalconfig " +
                "files. Using invalid values prevents the rule from being " +
                "applied as intended.",
            tags: [".globalconfig", "diagnostic", "severity"],
            category: Category.Bug);

        public static DiagnosticDescriptor UseExplicitSeverityLevel => New(
            id: 4028,
            title: "Use explicit diagnostic severity level",
            message: "Use explicit diagnostic severity level",
            description:
                "By explicitly setting is_global = true, the settings are correctly " +
                "applied globally across the entire project or solution, even if " +
                "the file uses a custom name instead of .globalconfig.",
            tags: [".globalconfig", "diagnostic", "severity"],
            category: Category.Clarity);

        public static DiagnosticDescriptor UseKnownDiagnosticIds => New(
            id: 4029,
            title: "Use valid diagnostic severity value",
            message: "Diagnostic analyzer rule '{0}' is unknown",
            description:
                "The compiler requires standard severity keywords to parse .globalconfig " +
                "files. Using invalid values prevents the rule from being " +
                "applied as intended.",
            tags: [".globalconfig", "diagnostic", "severity"],
            category: Category.Bug);

        public static DiagnosticDescriptor HeaderMustBeGlob => New(
            id: 4050,
            title: "Header must be a GLOB",
            message: "Header [{0}] is not a valid GLOB",
            description:
                ".editorconfig files work on the premise that header texts are " +
                "GLOB's matching files the key-value pairs of the section apply " +
                "to. Therefore, they must be valid GLOBs.",
            tags: [".editorconfig", "GLOB"],
            category: Category.SyntaxError);

        public static DiagnosticDescriptor UseEqualsAssign => New(
            id: 4051,
            title: "Use equals sign for key-value assignments",
            message: "Use '=' instead",
            description: "In .editorconfig files instead of : use = as assignment sign.",
            tags: ["INI", ".editorconfig"],
            category: Category.Clarity);

        public static DiagnosticDescriptor UseGlobalConfigForRoslynDiagnostics => New(
            id: 4052,
            title: "Use global configuration for Roslyn diagnostics",
            message: "Move entry {0} to the globalconfig",
            description:
                "Configuration entries intended to configure Roslyn " +
                "should all be put in a the globalconfig (most " +
                "likely a .globalconfig file).",
            tags: ["INI", ".editorconfig", "globalconfig", "Roslyn", "diagnostics"],
            category: Category.Clarity);
    }
}
