namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefineIsPackable() : MsBuildProjectFileAnalyzer(Rule.DefineIsPackable)
{
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (!context.File.IsTestProject() &&
            context.File.Property<IsPackable>() is null)
        {
            context.ReportDiagnostic(Descriptor, context.File);
        }
    }
}
