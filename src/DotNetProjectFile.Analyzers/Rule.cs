#pragma warning disable SA1118 // Parameter should not span multiple lines: readability for descriptions.

namespace DotNetProjectFile;

public static class Rule
{
    public static DiagnosticDescriptor ProjectFileCouldNotBeLocated => New(
        id: 0001,
        title: "MS Build project file could not be located",
        message: "The project file '{0}' could not be located.",
        description: "In order to make these rules work, the project file should be located.",
        tags: ["Configuration"],
        category: Category.Configuration);

    public static DiagnosticDescriptor UpdateLegacyProjects => New(
        id: 0002,
        title: "Upgrade legacy MS Build project files",
        message: "Upgrade legacy MS Build project file.",
        description: "MS Build legacy projects are not supported.",
        tags: ["project file", "legacy", "obsolete"],
        category: Category.Obsolete);

    public static DiagnosticDescriptor DefineUsingsExplicit => New(
        id: 0003,
        title: "Define usings explicit",
        message: "Define usings explicit.",
        description:
            "The included namespaces should be clear. To reduce the statements " +
            "per file, consider global using statements.",
        tags: ["Configuration", "usings", "global"],
        category: Category.Clarity);

    public static DiagnosticDescriptor RunNuGetSecurityAudit => New(
        id: 0004,
        title: "Run NuGet security audits automatically",
        message: "Run NuGet security audits automatically.",
        description:
            "To reduce security issues, NuGets security audit should be run on " +
            "every build automatically.",
        tags: ["security", "NuGet", "vulnerability"],
        category: Category.Security);

    public static DiagnosticDescriptor DefinePackageReferenceAssetsAsAttributes => New(
        id: 0005,
        title: "Define package reference assets as attributes",
        message: "Define package reference assets of '{0}' as attributes.",
        description:
            "To reduce security issues, NuGets security audit should be run on " +
            "every build automatically.",
        tags: ["security", "NuGet", "vulnerability"],
        category: Category.Clarity);

    public static DiagnosticDescriptor AddAdditionalFile => New(
        id: 0006,
        title: "Add additional files to improve static code analysis",
        message: "Add '{0}' to the additional files.",
        description:
            "By adding additional non-compiling files, those files become " +
            "available in the analyzer context too.",
        tags: ["code analysis"],
        category: Category.CodeQuality);

    public static DiagnosticDescriptor RemoveEmptyNodes => New(
        id: 0007,
        title: "Remove empty nodes",
        message: "Remove empty {0} node.",
        description: "Empty nodes only add noise, as they contain no information.",
        tags: ["noise"],
        category: Category.Noise);

    public static DiagnosticDescriptor RemoveFolderNodes => New(
        id: 0008,
        title: "Remove folder nodes",
        message: "Remove folder node '{0}'.",
        description:
            "Folders nodes only add noise. They are leftovers of directories " +
            "created in the IDE, without adding an actual file to it.",
        tags: ["noise"],
        category: Category.Noise);

    public static DiagnosticDescriptor DefineSingleTargetFramework => New(
        id: 0009,
        title: "Use the <TargetFramework> node for a single target framework",
        message: "Use the <TargetFramework> node instead.",
        description:
            "To prevent confusion, only use the <TargetFrameworks> node when " +
            "there are multiple target frameworks.",
        tags: ["target framework", "confusion"],
        category: Category.Clarity);

    public static DiagnosticDescriptor DefineOutputType => New(
        id: 0010,
        title: "Define the project output type explicitly",
        message: "Define the <OutputType> node explicitly.",
        description:
            "To prevent confusion, explicitly define the OutputType " +
            "as 'Library', 'Exe', 'WinExe' or 'Module'.",
        tags: ["Configuration", "confusion"],
        category: Category.Clarity);

    public static DiagnosticDescriptor DefinePropertiesOnce => New(
        id: 0011,
        title: "Define properties once",
        message: "Property <{0}> has been already defined.",
        description: "MS Build will only select one value of a property.",
        tags: ["Configuration", "confusion"],
        category: Category.Clarity);

    public static DiagnosticDescriptor ReassignPropertiesWithDifferentValue => New(
        id: 0012,
        title: "Reassign properties with a different value",
        message: "Property <{0}> has been previously defined with the same value.",
        description: "Reassigning a property with the same value is considered noise.",
        tags: ["Configuration", "confusion"],
        category: Category.Clarity);

    public static DiagnosticDescriptor IncludePackageReferencesOnce => New(
        id: 0013,
        title: "Include package references only once",
        message: "Package '{0}' is already referenced.",
        description: "Including package references twice is considered noise.",
        tags: ["Configuration", "confusion"],
        category: Category.Clarity);

    public static DiagnosticDescriptor IncludeProjectReferencesOnce => New(
        id: 0014,
        title: "Include project references only once",
        message: "Project '{0}' is already referenced.",
        description: "Including project references twice is considered noise.",
        tags: ["Configuration", "confusion"],
        category: Category.Clarity);

    public static DiagnosticDescriptor OrderPackageReferencesAlphabetically => New(
        id: 0015,
        title: "Order package references alphabetically",
        message: "Package '{0}' is not ordered alphabetically and should appear before '{1}'.",
        description: "Not ordering package references alphabetically is considered noise.",
        tags: ["Configuration", "confusion"],
        category: Category.Clarity);

    public static DiagnosticDescriptor OrderProjectReferencesAlphabetically => New(
        id: 0016,
        title: "Order project references alphabetically",
        message: "Project '{0}' is not ordered alphabetically and should appear before '{1}'.",
        description: "Not ordering project references alphabetically is considered noise.",
        tags: ["Configuration", "confusion"],
        category: Category.Clarity);

    public static DiagnosticDescriptor StaticAliasUsingNotSupported => New(
        id: 0017,
        title: "Can't create alias for static using directive",
        message: "Using directive for '{0}' can not be both an alias and static.",
        description: "Using directives can not be both static and an alias.",
        tags: ["Bug", "Code Generation"],
        category: Category.Bug,
        severity: DiagnosticSeverity.Error);

    public static DiagnosticDescriptor OrderUsingDirectivesByType => New(
        id: 0018,
        title: "Order using directives by type",
        message: "{0} directive for '{1}' should appear before {2} directive for '{3}'.",
        description: "Not ordering using directives by type is considered noise.",
        tags: ["Configuration", "confusion"],
        category: Category.Clarity);

    public static DiagnosticDescriptor OrderUsingDirectivesAlphabetically => New(
        id: 0019,
        title: "Order using directives alphabetically",
        message: "{0} directive '{1}' is not ordered alphabetically and should appear before '{2}'.",
        description: "Not ordering using directives alphabetically is considered noise.",
        tags: ["Configuration", "confusion"],
        category: Category.Clarity);

    public static DiagnosticDescriptor ItemGroupShouldBeUniform => New(
        id: 0020,
        title: "Item group should only contain nodes of a single type",
        message: "<ItemGroup> should only contain nodes of a single type.",
        description: "Mixing nodes of different types in a single <ItemGroup> is considered noise.",
        tags: ["Configuration", "confusion"],
        category: Category.Clarity);

    public static DiagnosticDescriptor BuildActionsShouldHaveSingleTask => New(
        id: 21,
        title: "Build actions should have a single task",
        message: "The <{0}> defines multiple tasks.",
        description:
            "For readability, a build action should define only one out of the options " +
            "Include, Exclude, Remove, or Update.",
        tags: ["Readability"],
        category: Category.Clarity);

    public static DiagnosticDescriptor BuildActionIncludeShouldExist => New(
       id: 22,
       title: "Build action includes should exist",
       message: "The Include '{0}' of <{1}> does not {2}.",
       description:
        "Build action include statements that do not include any file, are most " +
        "likely a left over, or a bug.",
       tags: [],
       category: Category.Noise);

    public static DiagnosticDescriptor DefineIsPackable => New(
        id: 0200,
        title: "Define the project packability explicitly",
        message: "Define the <IsPackable> node explicitly.",
        description:
            "To prevent confusion, explicitly define the " +
            "<IsPackable> node.",
        tags: ["Configuration", "confusion"],
        category: Category.Clarity);

    public static DiagnosticDescriptor DefineVersion => New(
        id: 0201,
        title: "Define the project version explicitly",
        message: "Define the <Version> node explicitly or define the <IsPackable> node with value 'false'.",
        description:
            "To ensure the creation of well-formed packages, " +
            "explicitly define the <Version> node or " +
            "disable package generation by defining the " +
            "<IsPackable> node with value 'false'.",
        tags: ["Configuration", "NuGet", "package"],
        category: Category.Reliability);

    public static DiagnosticDescriptor DefineDescription => New(
        id: 0202,
        title: "Define the project description explicitly",
        message: "Define the <Description> or <PackageDescription> node explicitly or define the <IsPackable> node with value 'false'.",
        description:
            "To ensure the creation of well-formed packages, explicitly define " +
            "the <Description> or <PackageDescription> node or disable package " +
            "generation by defining the <IsPackable> node with value 'false'.",
        tags: ["Configuration", "NuGet", "package"],
        category: Category.Configuration);

    public static DiagnosticDescriptor DefineAuthors => New(
        id: 0203,
        title: "Define the project authors explicitly",
        message: "Define the <Authors> node explicitly or define the <IsPackable> node with value 'false'.",
        description:
            "To ensure the creation of well-formed packages, " +
            "explicitly define the <Authors> node or " +
            "disable package generation by defining the " +
            "<IsPackable> node with value 'false'.",
        tags: ["Configuration", "NuGet", "package"],
        category: Category.Configuration);

    public static DiagnosticDescriptor DefineTags => New(
        id: 0204,
        title: "Define the project tags explicitly",
        message: "Define the <PackageTags> node explicitly or define the <IsPackable> node with value 'false'.",
        description:
            "To ensure the creation of well-formed packages, " +
            "explicitly define the <PackageTags> node or " +
            "disable package generation by defining the " +
            "<IsPackable> node with value 'false'.",
        tags: ["Configuration", "NuGet", "package"],
        category: Category.Configuration);

    public static DiagnosticDescriptor DefineRepositoryUrl => New(
        id: 0205,
        title: "Define the project repository URL explicitly",
        message: "Define the <RepositoryUrl> node explicitly or define the <IsPackable> node with value 'false'.",
        description:
            "To ensure the creation of well-formed packages, " +
            "explicitly define the <RepositoryUrl> node or " +
            "disable package generation by defining the " +
            "<IsPackable> node with value 'false'.",
        tags: ["Configuration", "NuGet", "package"],
        category: Category.Configuration);

    public static DiagnosticDescriptor DefineUrl => New(
        id: 0206,
        title: "Define the project URL explicitly",
        message: "Define the <PackageProjectUrl> node explicitly or define the <IsPackable> node with value 'false'.",
        description:
            "To ensure the creation of well-formed packages, " +
            "explicitly define the <PackageProjectUrl> node or " +
            "disable package generation by defining the " +
            "<IsPackable> node with value 'false'.",
        tags: ["Configuration", "NuGet", "package"],
        category: Category.Configuration);

    public static DiagnosticDescriptor DefineCopyright => New(
        id: 0207,
        title: "Define the project copyright explicitly",
        message: "Define the <Copyright> node explicitly or define the <IsPackable> node with value 'false'.",
        description:
            "To ensure the creation of well-formed packages, " +
            "explicitly define the <Copyright> node or " +
            "disable package generation by defining the " +
            "<IsPackable> node with value 'false'.",
        tags: ["Configuration", "NuGet", "package"],
        category: Category.Configuration);

    public static DiagnosticDescriptor DefineReleaseNotes => New(
        id: 0208,
        title: "Define the project release notes explicitly",
        message: "Define the <PackageReleaseNotes> node explicitly or define the <IsPackable> node with value 'false'.",
        description:
            "To ensure the creation of well-formed packages, " +
            "explicitly define the <PackageReleaseNotes> node or " +
            "disable package generation by defining the " +
            "<IsPackable> node with value 'false'.",
        tags: ["Configuration", "NuGet", "package"],
        category: Category.Configuration);

    public static DiagnosticDescriptor DefineReadmeFile => New(
        id: 0209,
        title: "Define the project readme file explicitly",
        message: "Define the <PackageReadmeFile> node explicitly or define the <IsPackable> node with value 'false'.",
        description:
            "To ensure the creation of well-formed packages, " +
            "explicitly define the <PackageReadmeFile> node or " +
            "disable package generation by defining the " +
            "<IsPackable> node with value 'false'.",
        tags: ["Configuration", "NuGet", "package"],
        category: Category.Configuration);

    public static DiagnosticDescriptor DefineLicense => New(
        id: 0210,
        title: "Define the project license expression explicitly",
        message: "Define the <PackageLicenseExpression> or <PackageLicenseFile> node explicitly or define the <IsPackable> node with value 'false'.",
        description:
            "To ensure the creation of well-formed packages " +
            "and for maximum compatibility with external tools, " +
            "explicitly define the <PackageLicenseExpression> " +
            "or <PackageLicenseFile>` node. Alternatively " +
            "disable package generation by defining the " +
            "<IsPackable> node with value 'false'.",
        tags: ["Configuration", "NuGet", "package"],
        category: Category.Configuration);

    public static DiagnosticDescriptor AvoidLicenseUrl => New(
        id: 0211,
        title: "Avoid using deprecated license definition",
        message: "Replace deprecated <PackageLicenseUrl> with <PackageLicenseExpression> or <PackageLicenseFile> node.",
        description:
            "The <PackageLicenseUrl> node has been deprecated " +
            "and should be replaced by either the " +
            "<PackageLicenseExpression> or the <PackageLicenseFile> node.",
        tags: ["Configuration", "NuGet", "package"],
        category: Category.Obsolete);

    public static DiagnosticDescriptor DefineIcon => New(
        id: 0212,
        title: "Define the project icon file explicitly",
        message: "Define the <PackageIcon> node explicitly or define the <IsPackable> node with value 'false'.",
        description:
            "To ensure the creation of well-formed packages " +
            "and for maximum compatibility with external tools, " +
            "explicitly define the <PackageIcon> node or " +
            "disable package generation by defining the " +
            "<IsPackable> node with value 'false'.",
        tags: ["Configuration", "NuGet", "package"],
        category: Category.Configuration);

    public static DiagnosticDescriptor DefineIconUrl => New(
        id: 0213,
        title: "Define the project icon URL explicitly",
        message: "Define the <PackageIconUrl> node explicitly or define the <IsPackable> node with value 'false'.",
        description:
            "To ensure the creation of well-formed packages " +
            "and for maximum compatibility with external tools, " +
            "explicitly define the deprecated <PackageIconUrl> node or " +
            "disable package generation by defining the " +
            "<IsPackable> node with value 'false'.",
        tags: ["Configuration", "NuGet", "package"],
        category: Category.Configuration);

    public static DiagnosticDescriptor DefinePackageId => New(
        id: 0214,
        title: "Define the NuGet project ID explicitly",
        message: "Define the <PackageId> node explicitly or define the <IsPackable> node with value 'false'.",
        description:
            "To ensure the creation of well-formed packages, " +
            "explicitly define the <PackageId> node or " +
            "disable package generation by defining the " +
            "<IsPackable> node with value 'false'.",
        tags: ["Configuration", "NuGet", "package"],
        category: Category.Configuration);

    public static DiagnosticDescriptor EnablePackageValidation => New(
        id: 0240,
        title: "Enable package validation",
        message: "Define the <EnablePackageValidation> node with value 'true', define the <IsPackable> node with value 'false' or define the <DevelopmentDependency> node with value 'false'.",
        description:
            "To ensure the (backwards) compatibility " +
            "of the API surface of your package, it is adviced " +
            "to enable package validation by defining the " +
            "<EnablePackageValidation> node with value 'true'.",
        tags: ["Configuration", "package", "compatibility"],
        category: Category.Reliability,
        severity: DiagnosticSeverity.Warning,
        isEnabled: true);

    public static DiagnosticDescriptor DefinePackageValidationBaselineVersion => New(
        id: 0241,
        title: "Enable package baseline validation",
        message: "Define the <PackageValidationBaselineVersion> node with a previously released stable version.",
        description:
            "To ensure the backwards compatibility " +
            "of the API surface of your package, it is adviced " +
            "to enable package baseline validation by defining the " +
            "<PackageValidationBaselineVersion> node with the version " +
            "of the previous stable release.",
        tags: ["Configuration", "package", "compatibility"],
        category: Category.Reliability,
        severity: DiagnosticSeverity.Warning,
        isEnabled: true);

    public static DiagnosticDescriptor DefineIsPublishable => New(
        id: 0400,
        title: "Define the project publishability explicitly",
        message: "Define the <IsPublishable> node explicitly.",
        description:
            "To prevent confusion, explicitly define the " +
            "<IsPublishable> node.",
        tags: ["Configuration", "confusion"],
        category: Category.Clarity);

    public static DiagnosticDescriptor AvoidGeneratePackageOnBuildWhenNotPackable => New(
        id: 600,
        title: "Avoid generating packages on build if not packable",
        message: "Avoid defining <GeneratePackageOnBuild> node explicitly when <IsPackable> is 'false'.",
        description:
            "The <GeneratePackageOnBuild> option has no effect " +
            "when <IsPackable> the node is disabled. " +
            "Removing the <GeneratePackageOnBuild> " +
            "node will reduce noise.",
        tags: ["Configuration", "NuGet", "package"],
        category: Category.Configuration);

    public static DiagnosticDescriptor UseDotNetProjectFileAnalyzers => New(
        id: 1000,
        title: "Use the .NET project file analyzers",
        message: "Use the .NET project file analyzers.",
        description: "To improve the code quality of .NET project files.",
        tags: ["roslyn", "analyzer", "cbproj", "vbproj"],
        category: Category.CodeQuality);

    public static DiagnosticDescriptor UseAnalyzersForPackages => New(
        id: 1001,
        title: "Use analyzers for packages",
        message: "Use {0} to analyze {1}.",
        description:
            "Some NuGet packages come with there own/dedicated Roslyn analyzers; " +
            "they just contain rules to improve the usage of those packages. " +
            "In order to get the best out of those NuGet packages, their " +
            "analyzer(s) should be used.",
        tags: ["roslyn", "analyzer", "NuGet"],
        category: Category.CodeQuality);

    public static DiagnosticDescriptor UseSonarAnalyzers => New(
        id: 1003,
        title: "Use Sonar analyzers for packages",
        message: "Add {0}.",
        description: "Improve the code quality by adding Sonar's Roslyn analyzers.",
        tags: ["roslyn", "analyzer", "NuGet", "Sonar"],
        category: Category.CodeQuality);

    public static DiagnosticDescriptor AvoidUsingMoq => New(
        id: 1100,
        title: "Avoid using Moq",
        message: "Do not use Moq.",
        description: "Moq has built in data harvesting that violates GDPR.",
        tags: ["GDPR", "privacy"],
        category: Category.Security);

    public static DiagnosticDescriptor ExcludePrivateAssetDependencies => New(
      id: 1200,
      title: "Exclude private assets as project file dependency",
      message: "Mark the package reference \"{0}\" as a private asset.",
      description:
          "Private assets (such as analyzers) will not result in being a " +
          "project dependency after being compiled.",
      tags: ["private", "asset", "dependencies", "dependency"],
      category: Category.CodeQuality);

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
        message: "Resource does not contain any data.",
        description: "A resource file without `<data>` elements is of no use.",
        tags: ["resx", "resources"],
        category: Category.Noise);

    public static DiagnosticDescriptor SortDataAlphabetically => New(
        id: 2002,
        title: "Sort resource file values alphabetically",
        message: "Resource '{0}' is not ordered alphabetically and should appear before '{1}'.",
        description:
            "To improve readability, and reduce the number of merge conflicts, " +
            "the `<data>` elements should be sorted based on the `@name` attribute.",
        tags: ["resx", "resources"],
        category: Category.Clarity);

    public static DiagnosticDescriptor AddInvariantFallbackResources => New(
        id: 2003,
        title: "Add invariant fallback resources",
        message: "Add invariant fallback resource.",
        description:
            "To ensure that localized values can be resolved, a localized resource " +
            "file must have a culture invariant alternative.",
        tags: ["resx", "resources", "invariant", "localization"],
        category: Category.Bug);

    public static DiagnosticDescriptor AddInvariantFallbackValues => New(
       id: 2004,
       title: "Add invariant fallback values",
       message: "Add invariant fallback value for '{0}'.",
       description:
           "To ensure that localized values can be resolved, a localized value " +
           "file must have a culture invariant alternative.",
       tags: ["resx", "resources", "invariant", "localization"],
       category: Category.Bug);

#pragma warning disable S107 // Methods should not have too many parameters
    // it calls a ctor with even more arguments.
    public static DiagnosticDescriptor New(
        int id,
        string title,
        string message,
        string description,
        string[] tags,
        Category category,
        DiagnosticSeverity severity = DiagnosticSeverity.Warning,
        bool isEnabled = true)
#pragma warning restore S107 // Methods should not have too many parameters
        => new(
            id: $"Proj{id:0000}",
            title: title,
            messageFormat: message,
            customTags: tags,
            category: category.ToString(),
            defaultSeverity: severity,
            isEnabledByDefault: isEnabled,
            description: description,
            helpLinkUri: $"https://dotnet-project-file-analyzers.github.io/rules/Proj{id:0000}.html");
}
