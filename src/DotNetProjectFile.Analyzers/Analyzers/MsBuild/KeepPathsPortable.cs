namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.KeepPathsPortable"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class KeepPathsPortable() : MsBuildProjectFileAnalyzer(Rule.KeepPathsPortable)
{
    /// <inheritdoc />
    public override ImmutableArray<AnalyzerType> ApplicableTo => ProjectFileTypes.All;

    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
        => Walk(context, context.File);

    private void Walk(ProjectFileAnalysisContext context, Node node)
    {
        foreach (var path in Paths(node).Where(path => IOPath.IsFullyQualified(path)))
            context.ReportDiagnostic(Descriptor, node, path);

        foreach (var child in node.Children)
            Walk(context, child);
    }

    private static IEnumerable<string> Paths(Node node) => node switch
    {
        Import import/*....................*/ => Any(import.Attribute("Project")),

        AdditionalFiles item/*.............*/ => item.IncludeAndUpdate,
        Compile item/*.....................*/ => item.IncludeAndUpdate,
        Content item/*.....................*/ => item.IncludeAndUpdate,
        EditorConfgFiles item/*............*/ => item.IncludeAndUpdate,
        EmbeddedResource item/*............*/ => item.IncludeAndUpdate,
        GlobalAnalyzerConfigFiles item/*...*/ => item.IncludeAndUpdate,
        None item/*........................*/ => item.IncludeAndUpdate,

        Folder item/*......................*/ => Any(item.Include),
        ProjectReference path/*............*/ => Any(path.Include),

        BaseIntermediateOutputPath path /*.*/ => Any(path.Text),
        BaseOutputPath path/*..............*/ => Any(path.Text),
        CodeAnalysisRuleSet path/*.........*/ => Any(path.Text),
        CscToolPath path/*.................*/ => Any(path.Text),
        DockerfileContext path/*...........*/ => Any(path.Text),
        DocumentationFile path/*...........*/ => Any(path.Text),
        DotnetFscCompilerPath path/*.......*/ => Any(path.Text),
        IntermediateOutputPath path/*......*/ => Any(path.Text),
        HintPath path/*....................*/ => Any(path.Text),
        OutputPath path/*..................*/ => Any(path.Text),
        PackageIcon path/*.................*/ => Any(path.Text),
        PackageLicenseFile path/*..........*/ => Any(path.Text),
        PackageOutputPath path/*...........*/ => Any(path.Text),
        PackageReadmeFile path/*...........*/ => Any(path.Text),
        PublishDir path/*..................*/ => Any(path.Text),
        VbcToolPath path/*.................*/ => Any(path.Text),

        _ => [],
    };

    private static IEnumerable<string> Any(string? path) => path is { Length: > 0 } ? [path] : [];
}
