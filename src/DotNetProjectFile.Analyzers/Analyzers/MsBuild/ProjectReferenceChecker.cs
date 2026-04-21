namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>
/// Implments <see cref="Rule.ProjectReferenceIncludeShouldExist"/>
/// and <see cref="Rule.ProjectReferenceIncludeDifferentCasing"/>
/// .</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class ProjectReferenceChecker() : MsBuildProjectFileAnalyzer(
    Rule.ProjectReferenceIncludeShouldExist,
    Rule.ProjectReferenceIncludeDifferentCasing)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        var root = context.File.Path.Directory;

        foreach (var node in context.File.Walk().OfType<ProjectReference>())
        {
            var include = node.Include ?? string.Empty;

            if (include is { Length: > 0 } && root.Files(include)?.FirstOrNone() is { Exists: true } existing)
            {
                if (IOPath.CaseCompare(existing, IOFile.Parse(include)) is { } casing)
                {
                    context.ReportDiagnostic(Rule.ProjectReferenceIncludeDifferentCasing, node, include, casing);
                }
            }
            else
            {
                context.ReportDiagnostic(Rule.ProjectReferenceIncludeShouldExist, node, include);
            }
        }
    }
}
