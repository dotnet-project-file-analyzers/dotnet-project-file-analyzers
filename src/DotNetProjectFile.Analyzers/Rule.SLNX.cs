#pragma warning disable SA1118 // Parameter should not span multiple lines: readability for descriptions.

namespace DotNetProjectFile;

public static partial class Rule
{
    public static class SLNX
    {
        public static DiagnosticDescriptor UseSlnxFiles => New(
            id: 5000,
            title: "Use SLNX solution files",
            message: "Use a SLNX solution file instead",
            description:
                "SLNX solution files are preferred over SLN files, for being less " +
                "verbose and easier to read.",
            tags: [Tag.SLNX],
            category: Category.Obsolete);

        public static DiagnosticDescriptor RemoveSlnFiles => New(
            id: 5001,
            title: "Remove SLN solution files",
            message: "Remove {0}",
            description:
                "SLNX solution files are preferred over SLN files, for being less " +
                "verbose and easier to read. As a result, the old SLN files should " +
                "be removed.",
            tags: [Tag.SLNX],
            category: Category.Noise);

        public static DiagnosticDescriptor OmitProjectIds => New(
            id: 5005,
            title: "Omit Project ID's",
            message: "Remove the Project ID",
            description: "ID's are left-overs from the SLN format, and can safely be omitted.",
            tags: [Tag.SLNX, "ID"],
            category: Category.Noise);

        public static DiagnosticDescriptor FileShouldExist => New(
           id: 5006,
           title: "Included files should exist",
           message: "Included file '{0}' does not exist",
           description: "Files included in a solution should exist on disk.",
           tags: [Tag.SLNX],
           category: Category.CodeSmell);

    }
}
