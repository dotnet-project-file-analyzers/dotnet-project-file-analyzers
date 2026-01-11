namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.PackablesShouldBeLibraries"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class PackablesShouldBeLibraries() : MsBuildProjectFileAnalyzer(Rule.PackablesShouldBeLibraries)
{
    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext<MsBuildProject> context)
    {
        if (!context.File.IsPackable() ||
            context.File.GetOutputType() == OutputType.Kind.Library ||
            IsTool(context.File, context.File.GetOutputType())) return;

        context.ReportDiagnostic(Descriptor, context.File);
    }

    private static bool IsTool(MsBuildProject project, OutputType.Kind outputType)
        => outputType == OutputType.Kind.Exe
        && project.Property<PackAsTool>()?.Value is true;
}
