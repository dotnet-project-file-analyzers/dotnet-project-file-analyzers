namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseInVBContextOnly()
    : MsBuildProjectFileAnalyzer(Rule.OverrideTargetFrameworksWithTargetFrameworks)
{
    private static readonly Type[] VBOnly =
    [
        typeof(FrameworkPathOverride),
        typeof(NoVBRuntimeReference),
        typeof(OptionExplicit),
        typeof(OptionInfer),
        typeof(OptionStrict,
        typeof(RemoveIntegerChecks),
        typeof(VbcToolPath),
        typeof(VbcVerbosity),
    ];

    protected override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile_DirectoryBuild;

    protected override void Register(ProjectFileAnalysisContext<MsBuildProject> context)
        => Walk(context.File, context);

    private void Walk(Node node, ProjectFileAnalysisContext<MsBuildProject> context)
    {
        if (VBOnly.Contains(node.GetType()))
        {
            context.ReportDiagnostic(Descriptor, node, node.LocalName);
        }

        foreach (var child in node.Children)
        {
            Walk(child, context);
        }
    }
}
