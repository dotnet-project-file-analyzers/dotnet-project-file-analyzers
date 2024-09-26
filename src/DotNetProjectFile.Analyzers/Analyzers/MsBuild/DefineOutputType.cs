namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefineOutputType() : MsBuildProjectFileAnalyzer(Rule.DefineOutputType)
{
    protected override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.File.Property<OutputType>() is null)
        {
            context.ReportDiagnostic(Descriptor, context.File);
        }
    }
}
