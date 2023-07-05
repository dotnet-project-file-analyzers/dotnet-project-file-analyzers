namespace Microsoft.CodeAnalysis.Diagnostics;

/// <summary>Extensions on <see cref="AnalysisContext"/>.</summary>
internal static class AnalysisContextExtensions
{
    /// <summary>Registers an action on <see cref="ProjectFileAnalysisContext"/>.</summary>
    public static void RegisterProjectFileAction(this AnalysisContext context, Action<ProjectFileAnalysisContext> action)
        => context.RegisterCompilationAction(c =>
        {
            var projects = Projects.Init(c);
            if (projects.EntryPoint(c.Compilation.Assembly) is { } project
                && string.IsNullOrEmpty(project.Element.Name.NamespaceName))
            {
                action.Invoke(new(project, c.Compilation, c.Options, c.CancellationToken, c.ReportDiagnostic));
            }
        });
}
