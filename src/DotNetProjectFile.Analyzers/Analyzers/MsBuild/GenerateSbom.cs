namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.GenerateSbom"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class GenerateSbom() : MsBuildProjectFileAnalyzer(Rule.GenerateSbom)
{
    private static readonly string Enable = "Enable SBOM generation with <GenerateSBOM> is 'true'";
    private static readonly string Package = $"Register the NuGet package '{NuGet.Packages.Microsoft_Sbom_Targets.Name}'";

    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (!context.File.IsPackable() || context.File.IsTestProject()) return;

        var property = context.File.Property<GenerateSBOM>();
        var registered = context.File
           .Walk().OfType<PackageReferenceBase>()
           .Where(p => p is PackageReference or GlobalPackageReference)
           .Any(NuGet.Packages.Microsoft_Sbom_Targets.IsMatch);

        if (property is { })
        {
            if (property.Value is not true)
            {
                context.ReportDiagnostic(Descriptor, property, Enable);
            }
            else if (!registered)
            {
                context.ReportDiagnostic(Descriptor, property, Package);
            }
        }
        else
        {
            context.ReportDiagnostic(Descriptor, context.File, Enable);
        }
    }
}
