namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class ProjectReferenceIncludeShouldExist() : MsBuildProjectFileAnalyzer(Rule.ProjectReferenceIncludeShouldExist)
{
    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        var root = context.File.Path.Directory;

        foreach (var node in context.File.Walk().OfType<ProjectReference>())
        {
            var include = node.Include ?? string.Empty;

            if (include is not { Length: > 0 } || root.Files(include)?.Any() == false)
            {
                context.ReportDiagnostic(Descriptor, node, include, node.LocalName);
            }
        }
    }
}
