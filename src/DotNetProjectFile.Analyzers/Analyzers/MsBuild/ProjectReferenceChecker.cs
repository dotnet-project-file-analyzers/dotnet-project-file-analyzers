namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>
/// Implments <see cref="Rule.ProjectReferenceIncludeShouldExist"/>
/// and <see cref="Rule.ProjectReferenceMustBeCaseConsistent"/>
/// .</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class ProjectReferenceChecker() : MsBuildProjectFileAnalyzer(
    Rule.ProjectReferenceIncludeShouldExist,
    Rule.ProjectReferenceMustBeCaseConsistent)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var node in context.File.Walk().OfType<ProjectReference>())
        {
            var include = node.Include ?? string.Empty;
            var (root, literal) = context.Resolve(node, include);

            if (include is { Length: > 0 } && root.Files(literal)?.FirstOrNone() is { Exists: true } existing)
            {
                if (IOPath.CaseCompare(existing, IOFile.Parse(literal)) is { } casing)
                {
                    context.ReportDiagnostic(Rule.ProjectReferenceMustBeCaseConsistent, node, include, casing);
                }
            }
            else
            {
                context.ReportDiagnostic(Rule.ProjectReferenceIncludeShouldExist, node, include);
            }
        }
    }
}
