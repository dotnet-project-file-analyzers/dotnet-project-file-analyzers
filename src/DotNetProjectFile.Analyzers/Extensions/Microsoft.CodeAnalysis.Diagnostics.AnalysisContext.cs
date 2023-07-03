using DotNetProjectFile.IO;

namespace Microsoft.CodeAnalysis.Diagnostics;

/// <summary>Extensions on <see cref="AnalysisContext"/>.</summary>
internal static class AnalysisContextExtensions
{
    /// <summary>Registers an action on <see cref="ProjectFileAnalysisContext"/>.</summary>
    public static void RegisterProjectFileAction(this AnalysisContext context, Action<ProjectFileAnalysisContext> action)
        => context.RegisterCompilationAction(c =>
        {
            if (ProjectFileResolver.Resolve(c) is { } project)
            {
                action.Invoke(new(project, c.Compilation, c.Options, c.CancellationToken, c.ReportDiagnostic));
            }
        });
}
