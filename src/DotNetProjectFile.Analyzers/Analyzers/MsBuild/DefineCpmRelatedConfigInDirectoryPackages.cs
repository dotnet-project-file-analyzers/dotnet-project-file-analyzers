using System.Collections.Frozen;

namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.DefineCpmRelatedConfigInDirectoryPackages"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefineCpmRelatedConfigInDirectoryPackages()
    : MsBuildProjectFileAnalyzer(Rule.DefineCpmRelatedConfigInDirectoryPackages)
{
    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.AllExceptDirectoryPackages;

    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc/>
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var cpmNode in context.File.DescendantsAndSelf().Where(IsCpm))
        {
            context.ReportDiagnostic(Descriptor, cpmNode, cpmNode.LocalName);
        }
    }

    private static bool IsCpm(Node node) => CpmTypes.Contains(node.GetType());

    private static readonly FrozenSet<Type> CpmTypes = new HashSet<Type>(
    [
        typeof(CentralPackageFloatingVersionsEnabled),
        typeof(CentralPackageTransitivePinningEnabled),
        typeof(CentralPackageVersionOverrideEnabled),
        typeof(GlobalPackageReference),
        typeof(PackageVersion),
    ])
    .ToFrozenSet();
}
