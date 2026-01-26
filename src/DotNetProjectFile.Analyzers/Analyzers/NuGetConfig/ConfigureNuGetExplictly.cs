namespace DotNetProjectFile.Analyzers.NuGetConfig;

/// <summary>Implements <see cref="Rule.NuGet.ConfigureNuGetExplictly"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class ConfigureNuGetExplictly() : MsBuildProjectFileAnalyzer(Rule.NuGet.ConfigureNuGetExplictly)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile_SDK;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext<MsBuildProject> context)
    {
        var dir = context.File.Path.Directory;

        while (dir.HasValue)
        {
            if (dir.Files() is { } files && files.Any(f => f.Name.IsMatch("NuGet.config"))) return;

            dir = dir.Parent;
        }

        context.ReportDiagnostic(Descriptor, context.File);
    }
}
