#pragma warning disable SA1118 // Parameter should not span multiple lines: readability for descriptions.

namespace DotNetProjectFile;

public static partial class Rule
{
    public static class NuGet
    {
        public static DiagnosticDescriptor ClearPreviousPackageSources => New(
            id: 0601,
            title: "Clear previous defined package sources",
            message: "Clear previous defined package sources",
            description: "A INI header should have the format [<Header>]",
            tags: ["NuGet"],
            category: Category.Security);
    }
}
