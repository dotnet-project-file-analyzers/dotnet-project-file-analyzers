namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseDirectoryPackagesPropsOnlyForCPM()
    : MsBuildProjectFileAnalyzer(Rule.OnlyUseDirectoryPackagesPropsForCPM)
{
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.DirectoryPackages;

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

        // Properties
        ManagePackageVersionsCentrally => true,
        CentralPackageFloatingVersionsEnabled => true,
        CentralPackageTransitivePinningEnabled => true,
        CentralPackageVersionOverrideEnabled => true,

        // Only self is allowed
        AdditionalFiles additional => additional.Include.Any(i => i.EndsWith("Directory.Packages.props")),
        _ => false,
    };
}
