namespace DotNetProjectFile.Analyzers;

/// <summary>
/// Base for <see cref="DiagnosticAnalyzer"/>s to analyze MS Build project files.
/// </summary>
public abstract class MsBuildProjectFileAnalyzer(
    DiagnosticDescriptor primaryDiagnostic,
    params DiagnosticDescriptor[] supportedDiagnostics)
    : ProjectFileAnalyzer<MsBuildProject>(primaryDiagnostic, supportedDiagnostics)
{
    /// <summary>
    /// Defines to which <see cref="ProjectFileType"/>s the rule is applicable.
    /// </summary>
    /// <remarks>
    /// Default is <see cref="ProjectFileTypes.All"/>.
    /// </remarks>
    public virtual ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.All;

    /// <summary>
    /// Defines to which <see cref="Language"/>s the rule is applicable.
    /// </summary>
    /// <remarks>
    /// Default is <see cref="Languages.All"/>.
    /// </remarks>
    public virtual ImmutableArray<Language> ApplicableLanguages { get; } = Languages.All;

    /// <summary>Indicates that the rule will not be executed once an import could not be resolved (default is true).</summary>
    public virtual bool DisableOnFailingImport => true;

    /// <summary>Registers the analyzer for all MS Build projects files.</summary>
    protected override void Register(AnalysisContext context)
        => context.RegisterProjectFileAction(c =>
        {
            if (ApplicableTo.Contains(c.File.FileType)
                && (ApplicableLanguages.Contains(c.File.Language) || c.File.Language == Language.None)
                && !(c.File.HasFailingImport && DisableOnFailingImport)
                && (!IsProjectFileWithinSdk(c) || IsSupportedSdkProject(c)))
            {
                Register(c);
            }
        });

    private bool IsSupportedSdkProject(in ProjectFileAnalysisContext project)
        => project.File.FileType is ProjectFileType.ProjectFile
        && ApplicableLanguages.Contains(project.File.Language);

    /// <remarks>
    /// We do not want to analyze the project files witin the context of the SDK
    /// as that would lead to projects being analyzed multiple times and
    /// potentially even to projects being analyzed that are not part of the
    /// solution.
    /// </remarks>
    private static bool IsProjectFileWithinSdk(in ProjectFileAnalysisContext context)
        => context.File.FileType == ProjectFileType.ProjectFile
        && context.Compilation.AssemblyName == ".net";
}
