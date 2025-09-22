using DotNetProjectFile.MsBuild.VisualBasic;

namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseInVBContextOnly() : MsBuildProjectFileAnalyzer(Rule.UseInVBContextOnly)
{
    private static readonly Type[] VBOnly =
    [
        typeof(FrameworkPathOverride),
        typeof(NoVBRuntimeReference),
        typeof(OptionExplicit),
        typeof(OptionInfer),
        typeof(OptionStrict),
        typeof(RemoveIntegerChecks),
        typeof(VbcToolPath),
        typeof(VbcVerbosity),
    ];

    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile_DirectoryBuild;

    protected override void Register(ProjectFileAnalysisContext<MsBuildProject> context)
    {
        if (!InVBContext(context.File))
        {
            Walk(context.File, context);
        }
    }

    private static bool InVBContext(MsBuildProject project) => project.FileType switch
    {
        ProjectFileType.ProjectFile => project.Path.Extension.IsMatch(Language.CSharp.ProjectFile),
        ProjectFileType.DirectoryBuild => project.Path.Directory.Files($"*{Language.VisualBasic.ProjectFile}").Any(),
        _ => false,
    };

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
