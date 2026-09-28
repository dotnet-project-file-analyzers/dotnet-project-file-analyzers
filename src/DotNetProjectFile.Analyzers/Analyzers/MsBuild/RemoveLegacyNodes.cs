namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class RemoveLegacyNodes() : MsBuildProjectFileAnalyzer(Rule.RemoveLegacyNodes)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    public override ImmutableArray<AnalyzerType> ApplicableTo => ProjectFileTypes.All;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context) => Walk(context, context.File);

    private void Walk(ProjectFileAnalysisContext context, Node node)
    {
        if (node
            is AppDesignerFolder
            or AssemblyCompany
            or AssemblyConfiguration
            or AssemblyCopyright
            or AssemblyDescription
            or AssemblyFileVersion
            or AssemblyInformationalVersion
            or AssemblyProduct
            or AssemblyTitle
            or AssemblyVersion
            or BootstrapperPackage
            or FileAlignment
            or HintPath
            or IISExpressSSLPort
            or Install
            or InstallFrom
            or NuGetPackageImportStamp
            or ProjectExtensions
            or ProjectGuid
            or ProjectTypeGuids
            or SchemaVersion
            or SlnVersion
            or StartAction
            or TargetFrameworkProfile
            or TargetFrameworkVersion
            or TargetPlatformIdentifier
            or TargetPlatformMinVersion
            or TargetPlatformVersion
            or UseGlobalApplicationHostFile
            or UseIISExpress
            or UseVSToolPath)
        {
            context.ReportDiagnostic(Descriptor, node, node.LocalName);
        }

        foreach (var child in node.Children)
            Walk(context, child);
    }
}
