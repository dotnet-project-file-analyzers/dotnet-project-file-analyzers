namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.UseSonarAnalyzers"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseSonarAnalyzers() : MsBuildProjectFileAnalyzer(Rule.UseSonarAnalyzers)
{
    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (Include(context.File.Language) is not { } include) { return; }

        if (context.File
            .Walk()
            .OfType<PackageReferenceBase>()
            .None(p => p is not PackageVersion && p.Include.IsMatch(include)))
        {
            context.ReportDiagnostic(Descriptor, context.File, include);
        }
    }

    private static string? Include(Language language)
    {
        if (language == Language.CSharp)
        {
            return "SonarAnalyzer.CSharp";
        }
        else if (language == Language.VisualBasic)
        {
            return "SonarAnalyzer.VisualBasic";
        }
        else
        {
            return null;
        }
    }
}
