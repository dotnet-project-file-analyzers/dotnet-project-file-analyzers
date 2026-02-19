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
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    public override ImmutableArray<Language> ApplicableLanguages { get; } = Languages.All.Remove(Language.CSharp);

    protected override void Register(ProjectFileAnalysisContext context) => Walk(context.File, context);

    private void Walk(Node node, ProjectFileAnalysisContext context)
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
