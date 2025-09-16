namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.DefineOutputType"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic, LanguageNames.FSharp)]
public sealed class DefineOutputType() : MsBuildProjectFileAnalyzer(Rule.DefineOutputType)
{
    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.File.Property<OutputType>() is null)
        {
            context.ReportDiagnostic(Descriptor, context.File);
        }
    }
}
