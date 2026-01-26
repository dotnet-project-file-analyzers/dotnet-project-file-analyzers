#pragma warning disable SA1118 // Parameter should not span multiple lines: readability for descriptions.

namespace DotNetProjectFile;

public static partial class Rule
{
    public static class NuGet
    {
        public static DiagnosticDescriptor ConfigureNuGetExplictly => New(
            id: 0300,
            title: "Configure NuGet explicitly",
            message: "A NuGet.config file could not be resolved",
            description:
                "The only way to ensure that NuGet is setup safely is by adding " +
                "a NuGet.config file that is part of the source code.",
            tags: ["NuGet"],
            category: Category.Security);

        public static DiagnosticDescriptor ClearPreviousPackageSources => New(
            id: 0301,
            title: "Clear previously defined package sources",
            message: "Clear previously defined package sources",
            description:
                "To prevent supply chain attacks no package sources should be " +
                "inherited from globally defined sources that could contain " +
                "malicious sources.",
            tags: ["NuGet"],
            category: Category.Security);
    }
}
