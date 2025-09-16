using System.Collections.Frozen;
using System.Reflection;

namespace DotNetProjectFile.Analyzers;

/// <summary>
/// Base for <see cref="DiagnosticAnalyzer"/>s to analyze MS Build project files.
/// </summary>
public abstract class MsBuildProjectFileAnalyzer : ProjectFileAnalyzer<MsBuildProject>
{
    private readonly FrozenSet<string> supportedNonRoslynLanguages;

    protected MsBuildProjectFileAnalyzer(
        DiagnosticDescriptor primaryDiagnostic,
        params DiagnosticDescriptor[] supportedDiagnostics)
        : base(primaryDiagnostic, supportedDiagnostics)
    {
        supportedNonRoslynLanguages = GetType()
            .GetCustomAttributes<DiagnosticAnalyzerAttribute>()
            .SelectMany(static attr => attr.Languages)
            .Where(static lang => lang is not LanguageNames.CSharp and not LanguageNames.VisualBasic)
            .ToFrozenSet();
    }

    /// <summary>
    /// Defines to which <see cref="ProjectFileType"/>s the rule is applicable.
    /// </summary>
    /// <remarks>
    /// Default is <see cref="ProjectFileTypes.All"/>.
    /// </remarks>
    public virtual IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.All;

    /// <summary>Indicates that the rule will not be executed once an import could not be resolved (default is true).</summary>
    public virtual bool DisableOnFailingImport => true;

    /// <summary>Registers the analyzer for all MS Build projects files.</summary>
    protected override void Register(AnalysisContext context)
        => context.RegisterProjectFileAction(c =>
        {
            if (ApplicableTo.Contains(c.File.FileType)
                && !(c.File.HasFailingImport && DisableOnFailingImport)
                && (!IsProjectFileWithinSdk(c) || IsSupportedNonRoslynProject(c)))
            {
                Register(c);
            }
        });

    private bool IsSupportedNonRoslynProject(in ProjectFileAnalysisContext project)
        => project.File.FileType is ProjectFileType.ProjectFile
        && project.File.LanguageName is { Length: > 0 } language
        && supportedNonRoslynLanguages.Contains(language);

    /// <remarks>
    /// We do not want to analyze the project files witin the context of the SDK
    /// as that is that would lead pt projects being analyzed multiple times and
    /// potentially even to projects being analyzed that are not part of the
    /// solution.
    /// </remarks>
    private static bool IsProjectFileWithinSdk(in ProjectFileAnalysisContext context)
        => context.File.FileType == ProjectFileType.ProjectFile
        && context.Compilation.AssemblyName == ".net";
}
