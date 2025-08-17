#pragma warning disable SA1118 // Parameter should not span multiple lines: readability for descriptions.

namespace DotNetProjectFile;

public static partial class Rule
{
    public static class RESX
    {
        public static DiagnosticDescriptor EmbedValidResourceFiles => New(
            id: 2000,
            title: "Embed valid resource files",
            message: "Resource file {0}",
            description: "A resource file should contain valid XML.",
            tags: ["resx", "resources"],
            category: Category.Bug,
            severity: DiagnosticSeverity.Error);

        public static DiagnosticDescriptor DefineData => New(
            id: 2001,
            title: "Define data in a resource file",
            message: "Resource does not contain any data",
            description: "A resource file without `<data>` elements is of no use.",
            tags: ["resx", "resources"],
            category: Category.Noise);

        public static DiagnosticDescriptor SortDataAlphabetically => New(
            id: 2002,
            title: "Sort resource file values alphabetically",
            message: "Resource '{0}' is not ordered alphabetically and should appear before '{1}'",
            description:
                "To improve readability, and reduce the number of merge conflicts, " +
                "the `<data>` elements should be sorted based on the `@name` attribute.",
            tags: ["resx", "resources"],
            category: Category.Clarity);

        public static DiagnosticDescriptor AddInvariantFallbackResources => New(
            id: 2003,
            title: "Add invariant fallback resources",
            message: "Add invariant fallback resource",
            description:
                "To ensure that localized values can be resolved, a localized resource " +
                "file must have a culture invariant alternative.",
            tags: ["resx", "resources", "invariant", "localization"],
            category: Category.Bug);

        public static DiagnosticDescriptor AddInvariantFallbackValues => New(
           id: 2004,
           title: "Add invariant fallback values",
           message: "Add invariant fallback value for '{0}'",
           description:
               "To ensure that localized values can be resolved, a localized value " +
               "file must have a culture invariant alternative.",
           tags: ["resx", "resources", "invariant", "localization"],
           category: Category.Bug);

        public static DiagnosticDescriptor EscapeXmlNodesResourceValues => New(
          id: 2005,
          title: "Escape XML nodes of resource values",
          message: "Escape the XML node in '{0}'",
          description: "To ensure correct handling, XML nodes within resource values should be escaped.",
          tags: ["resx", "resources", "XML", "escaping"],
          category: Category.Bug);

        public static DiagnosticDescriptor Indent => New(
          id: 2100,
          title: "Indent XML files",
          message: "The element <{0}> has not been properly indented",
          description: "To improve readability, XML elements should be properly indented.",
          tags: ["XML", "indentation"],
          category: Category.Formatting);
    }
}
