namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseInCSharpContextOnly() : MsBuildProjectFileAnalyzer(Rule.UseInCSharpContextOnly)
{
    private static readonly Type[] CSharpOnly =
    [
        typeof(DotNetProjectFile.MsBuild.CSharp.AllowUnsafeBlocks),
        typeof(DotNetProjectFile.MsBuild.CSharp.CheckForOverflowUnderflow),
        typeof(DotNetProjectFile.MsBuild.CSharp.Nullable),
    ];

    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile_DirectoryBuild;

    protected override void Register(ProjectFileAnalysisContext<MsBuildProject> context)
    {
        if (!InCSharpContext(context.File))
        {
            Walk(context.File, context);
        }
    }

    private static bool InCSharpContext(MsBuildProject project) => project.FileType switch
    {
        ProjectFileType.ProjectFile => project.Path.Extension.IsMatch(".csproj"),
        ProjectFileType.DirectoryBuild => project.Path.Directory.Files("*.vbproj")!.None(),
        _ => false,
    };

    private void Walk(Node node, ProjectFileAnalysisContext<MsBuildProject> context)
    {
        if (CSharpOnly.Contains(node.GetType()))
        {
            context.ReportDiagnostic(Descriptor, node, node.LocalName);
        }

        foreach (var child in node.Children)
        {
            Walk(child, context);
        }
    }
}
