namespace DotNetProjectFile.Analyzers.Slnx;

/// <summary>Handles the use of SLNX files.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseSlnxFiles() : MsBuildProjectFileAnalyzer(Rule.SLNX.UseSlnxFiles, Rule.SLNX.RemoveSlnFiles)
{
    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.SDK;

    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.Options.AdditionalFiles.None(f => IOFile.Parse(f.Path).Extension.IsMatch(".slnx")))
        {
            context.ReportDiagnostic(Rule.SLNX.UseSlnxFiles, context.File);
        }
        else
        {
            foreach (var sln in context.File.Path.Directory.Files("**/*.sln") ?? [])
            {
                context.ReportDiagnostic(Rule.SLNX.RemoveSlnFiles, sln.AsLocation(), sln.Name);
            }
        }
    }
}
