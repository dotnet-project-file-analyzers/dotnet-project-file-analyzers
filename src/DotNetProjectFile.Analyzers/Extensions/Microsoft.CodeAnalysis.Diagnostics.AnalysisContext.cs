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
                foreach (var p in project.AncestorsAndSelf())
                {
                    action.Invoke(new(p, c.Compilation, c.Options, c.CancellationToken, c.ReportDiagnostic));
                }
            }
        });

    /// <summary>Registers an action on <see cref="ProjectFileAnalysisContext"/>.</summary>
    public static void RegisterResourceFileAction(this AnalysisContext context, Action<ResourceFileAnalysisContext> action)
        => context.RegisterCompilationAction(c =>
        {
            foreach (var resource in DotNetProjectFile.Resx.Resources.Resolve(c.Options.AdditionalFiles))
            {
                action.Invoke(new(resource, c.Compilation, c.Options, c.CancellationToken, c.ReportDiagnostic));
            }
        });
}
