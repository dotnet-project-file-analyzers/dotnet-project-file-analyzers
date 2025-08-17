namespace DotNetProjectFile;

public static partial class Rule
{
    public static class SLNX
    {
        public static DiagnosticDescriptor OmitProjectIds => New(
            id: 5005,
            title: "Omit Project ID's",
            message: "Remove the Project ID",
            description: "ID's are left-overs from the SLN format, and can safely be ommitted.",
            tags: ["SLNX", "ID"],
            category: Category.Noise);
    }
}
