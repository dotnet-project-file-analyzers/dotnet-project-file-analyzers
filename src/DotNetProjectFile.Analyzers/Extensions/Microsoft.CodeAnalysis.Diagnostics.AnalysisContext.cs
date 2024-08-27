namespace Microsoft.CodeAnalysis.Diagnostics;

/// <summary>Extensions on <see cref="AnalysisContext"/>.</summary>
internal static class AnalysisContextExtensions
{
    /// <summary>Registers an action on <see cref="ProjectFileAnalysisContext"/>.</summary>
    public static void RegisterProjectFileAction(this AnalysisContext context, Action<ProjectFileAnalysisContext> action)
    {
        context.RegisterAdditionalFileAction(c =>
        {
            if (Projects.Init(c).EntryPoint(c) is { } project
                && string.IsNullOrEmpty(project.Element.Name.NamespaceName))
            {
                foreach (var p in project.ImportsAndSelf())
                {
                    action.Invoke(new(p, c.Compilation, c.Options, c.CancellationToken, c.ReportDiagnostic));
                }
            }
        });

        // Fallback for detecting files that not have been added as additional files.
        context.RegisterCompilationAction(c =>
        {
            if (Projects.Init(c).EntryPoint(c) is { IsAdditional: false } project
                && string.IsNullOrEmpty(project.Element.Name.NamespaceName))
            {
                foreach (var p in project.ImportsAndSelf())
                {
                    action.Invoke(new(p, c.Compilation, c.Options, c.CancellationToken, c.ReportDiagnostic));
                }
            }
        });
    }

    /// <summary>Registers an action on <see cref="ProjectFileAnalysisContext"/>.</summary>
    public static void RegisterResourceFileAction(this AnalysisContext context, Action<ResourceFileAnalysisContext> action)
        => context.RegisterAdditionalFileAction(c =>
        {
            foreach (var resource in DotNetProjectFile.Resx.Resources.Resolve(c.Compilation, c.Options.AdditionalFiles).Where(r => r.IsXml))
            {
                action.Invoke(new(resource, c.Compilation, c.Options, c.CancellationToken, c.ReportDiagnostic));
            }
        });
}
