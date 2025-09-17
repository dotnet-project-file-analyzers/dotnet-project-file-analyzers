namespace DotNetProjectFile.Analyzers.Slnx;

/// <summary>Implements <see cref="Rule.SLNX.UseSlnxFiles"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseSlnxFiles() : MsBuildProjectFileAnalyzer(Rule.SLNX.UseSlnxFiles)
{
    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.SDK;

    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.Options.AdditionalFiles.None(f => IOFile.Parse(f.Path).Extension.IsMatch(".slnx")))
        {
            context.ReportDiagnostic(Descriptor, context.File);
        }
    }
}
