namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class AddAdditionalFile() : MsBuildProjectFileAnalyzer(Rule.AddAdditionalFile)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (!context.Project.IsAdditional && FileTypes.Contains(context.Project.FileType))
        {
            context.ReportDiagnostic(Descriptor, context.Project, context.Project.Path.Name);
        }
    }

    private static readonly ProjectFileType[] FileTypes = [ProjectFileType.ProjectFile, ProjectFileType.Props];
}
