namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.DefineGlobalPackageReferenceInDirectoryPackagesOnly"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefineGlobalPackageReferenceInDirectoryPackagesOnly()
    : MsBuildProjectFileAnalyzer(Rule.DefineGlobalPackageReferenceInDirectoryPackagesOnly)
{
    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.AllExceptDirectoryPackages;

    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc/>
    protected override void Register(ProjectFileAnalysisContext<MsBuildProject> context)
    {
        foreach (var reference in context.File.ItemGroups.Children<GlobalPackageReference>())
        {
            context.ReportDiagnostic(Descriptor, reference);
        }
    }
}
