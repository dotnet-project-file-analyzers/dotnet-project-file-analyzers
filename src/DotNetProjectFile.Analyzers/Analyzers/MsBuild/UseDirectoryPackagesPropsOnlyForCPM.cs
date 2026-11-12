namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.OnlyUseDirectoryPackagesPropsForCPM" />.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseDirectoryPackagesPropsOnlyForCPM()
    : MsBuildProjectFileAnalyzer(Rule.OnlyUseDirectoryPackagesPropsForCPM)
{
    /// <inheritdoc />
    public override ImmutableArray<AnalyzerType> ApplicableTo => ProjectFileTypes.DirectoryPackages;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        Walk(context.File);

        void Walk(Node node)
        {
            if (!IsCPM(node))
            {
                context.ReportDiagnostic(Descriptor, node, node.LocalName);
            }

            foreach (var child in node.Children)
            {
                Walk(child);
            }
        }
    }

    private static bool IsCPM(Node node) => node switch
    {
        // General
        MsBuildProject => true,
        PropertyGroup => true,
        ItemGroup => true,
        PackageVersion => true,
        GlobalPackageReference => true,
        ThirdPartyLicense => true,

        // Properties
        ManagePackageVersionsCentrally => true,
        CentralPackageFloatingVersionsEnabled => true,
        CentralPackageTransitivePinningEnabled => true,
        CentralPackageVersionOverrideEnabled => true,

        // Only self is allowed
        AdditionalFiles additional => additional.Include.Any(i => i.EndsWith("Directory.Packages.props")),

        // Properties and Items containing "License"
        _ => node.LocalName.Contains("License", StringComparison.OrdinalIgnoreCase),
    };
}
