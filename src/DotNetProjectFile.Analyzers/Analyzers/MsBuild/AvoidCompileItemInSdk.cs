namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class AvoidCompileItemInSdk() : MsBuildProjectFileAnalyzer(Rule.AvoidCompileItemInSdk)
{
    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.SDK;

    protected override void Register(ProjectFileAnalysisContext<MsBuildProject> context)
    {
        foreach (var compile in context.File.ItemGroups
            .SelectMany(i => i.BuildActions)
            .OfType<Compile>()
            .Where(c => c.IncludeAndUpdate.Any()))
        {
            context.ReportDiagnostic(Descriptor, compile);
        }
    }
}
