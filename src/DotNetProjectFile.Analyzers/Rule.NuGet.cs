#pragma warning disable SA1118 // Parameter should not span multiple lines: readability for descriptions.

namespace DotNetProjectFile;

public static partial class Rule
{
    public static class NuGet
    {
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

        public static DiagnosticDescriptor CredentialsShouldBeInjected => New(
           id: 0302,
           title: "Credentials should be injected",
           message: "Use a placeholder to inject the password instead",
           description:
               "Credentials should not be exposed in repositories for obvious " +
               "reasons. Therefor they should be injected into file that " +
               "themeselves are part of the codebase.",
           tags: ["NuGet"],
           category: Category.Security);
    }
}
