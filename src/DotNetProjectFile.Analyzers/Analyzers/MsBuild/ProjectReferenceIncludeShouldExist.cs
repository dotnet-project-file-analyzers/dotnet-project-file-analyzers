namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class ProjectReferenceIncludeShouldExist() : MsBuildProjectFileAnalyzer(Rule.ProjectReferenceIncludeShouldExist)
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

            if (include is not { Length: > 0 } || root.Files(include)?.FirstOrDefault() is not { } existing)
            {
                context.ReportDiagnostic(Descriptor, node, include, node.LocalName);
            }
            else if (existing.Name != include)
            {
                // too find casing differences
            }
        }
    }
}
