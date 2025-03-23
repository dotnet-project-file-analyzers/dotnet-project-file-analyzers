namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class TrackToDoTags() : MsBuildProjectFileAnalyzer(Rule.TrackToDoTags)
{
    private readonly ToDoChecker<MsBuildProject> Checker = new(Rule.TrackToDoTags, GetText);

    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
        => Checker.Check(context.File, context.File.Text, context);

    private static string? GetText(XmlAnalysisNode node) => node switch
    {
        PropertyGroup g => g.Label,
        ItemGroup g => g.Label,
        _ => null,
    };
}
