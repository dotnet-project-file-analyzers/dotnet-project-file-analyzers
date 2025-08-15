namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.LabelItemGroupsThatRemoveCompilationItems"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class LabelItemGroupsThatRemoveCompilationItems()
    : MsBuildProjectFileAnalyzer(Rule.LabelItemGroupsThatRemoveCompilationItems)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.All;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext<MsBuildProject> context)
    {
        foreach (var group in context.File.ItemGroups.Where(RequireLabel))
        {
            context.ReportDiagnostic(Descriptor, group);
        }
    }

    private static bool RequireLabel(ItemGroup group)
        => group.Label is not { Length: > 0 }
        && group.Children.OfType<Compile>().Any(c => c.Remove is { Count: > 0 });
}
