#pragma warning disable SA1118 // Parameter should not span multiple lines: readability for descriptions.

using Grammr.Text;

namespace DotNetProjectFile;

public static partial class Rule
{
    public static class NuGet
    {
        public static DiagnosticDescriptor ConfigureNuGetExplicitly => New(
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

        public static DiagnosticDescriptor InjectCredentials => New(
           id: 0302,
           title: "Credentials should be injected",
           message: "Use a placeholder to inject the password instead",
           description:
               "Credentials should not be exposed in repositories for obvious " +
               "reasons. Therefor they should be injected into file that " +
               "themeselves are part of the codebase.",
           tags: ["NuGet"],
           category: Category.Security);

        public static DiagnosticDescriptor DefineMappingForMultipleSources => New(
            id: 0303,
            title: "Define a mapping for each package source",
            message: "The <packageSource key=\"{0}\"> is missing a <packageSourceMapping>",
            description:
                "To prevent supply chain attacks each <packageSource> should " +
                "have its own <packageSourceMapping>. By doing so, the origin " +
                "of packages is predictable.",
            tags: ["NuGet"],
            category: Category.Security);

        public static DiagnosticDescriptor PackageSourceMappingsShouldBeUnique => New(
            id: 0304,
            title: "Package source mappings should be unique",
            message: "The mapping '{0}' is not unique",
            description:
                "The first package source mapping that matches serves the package. " +
                "Therefor, a second mapping with the same pattern will never match.",
            tags: ["NuGet"],
            category: Category.Bug);

        public static DiagnosticDescriptor LastMappingCatchesAll => New(
            id: 0305,
            title: "Last source map should map all packages",
            message: "The last mapping should be '*' to match all",
            description:
                "Mappings are processed in ascending order, first to match serves. " +
                "To ensure all packages can be served by at least one package, " +
                "the last one should be a match all (*).",
            tags: ["NuGet"],
            category: Category.Bug);
    }
}
