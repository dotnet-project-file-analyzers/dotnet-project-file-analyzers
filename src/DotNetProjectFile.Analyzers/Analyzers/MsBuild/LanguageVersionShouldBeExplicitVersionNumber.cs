namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.LanguageVersionShouldBeExplicitVersionNumber"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class LanguageVersionShouldBeExplicitVersionNumber() : MsBuildProjectFileAnalyzer(Rule.LanguageVersionShouldBeExplicitVersionNumber)
{
    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext<MsBuildProject> context)
    {
        if (context.File.Property<LangVersion>() is not { } node)
        {
            context.ReportDiagnostic(Descriptor, context.File);
        }
        else if (!node.Value.IsExplicit)
        {
            context.ReportDiagnostic(Descriptor, node);
        }
    }
}
