namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class AddAdditionalFile() : MsBuildProjectFileAnalyzer(Rule.AddAdditionalFile)
{
    protected override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile_Props;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (!context.File.IsAdditional(context.Options.AdditionalFiles))
        {
            context.ReportDiagnostic(Descriptor, context.File, context.File.Path.Name);
        }
    }
}
