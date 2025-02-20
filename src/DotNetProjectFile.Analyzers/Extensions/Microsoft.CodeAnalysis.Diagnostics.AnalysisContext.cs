namespace Microsoft.CodeAnalysis.Diagnostics;

/// <summary>Extensions on <see cref="AnalysisContext"/>.</summary>
internal static class AnalysisContextExtensions
{
    /// <summary>Registers an action on <see cref="ProjectFileAnalysisContext"/>.</summary>
    public static void RegisterProjectFileAction(this AnalysisContext context, Action<ProjectFileAnalysisContext> action)
    {
        context.RegisterAdditionalFileAction(c =>
        {
            if (ProjectFiles.Global.UpdateMsBuildProject(c) is { IsLegacy: false } msbuild)
            {
                action.Invoke(new(msbuild, c.Compilation, c.Options, c.CancellationToken, c.ReportDiagnostic));
            }
        });

        // Fallback for detecting files that not have been added as additional files.
        context.RegisterCompilationAction(c =>
        {
            if (ProjectFiles.Global.UpdateMsBuildProject(c) is { IsLegacy: false } entry)
            {
                foreach (var msbuild in entry.ImportsAndSelf().Where(x => !x.IsAdditional(c.Options.AdditionalFiles)))
                {
                    action.Invoke(new(msbuild, c.Compilation, c.Options, c.CancellationToken, c.ReportDiagnostic));
                }
            }
        });
    }

    /// <summary>Registers an action on <see cref="ProjectFileAnalysisContext"/>.</summary>
    public static void RegisterEditorConfigFileAction(this AnalysisContext context, Action<IniFileAnalysisContext> action)
    {
        context.RegisterAdditionalFileAction(c =>
        {
            if (ProjectFiles.Global.UpdateIniFile(c) is { } ini)
            {
                action.Invoke(new(ini, c.Compilation, c.Options, c.CancellationToken, c.ReportDiagnostic));
            }
        });

        // Fallback for detecting files that not have been added as additional files.
        context.RegisterCompilationAction(c =>
        {
            if (ProjectFiles.Global.UpdateMsBuildProject(c) is { } msbuild)
            {
                foreach (var config in msbuild.EditorConfigs().Where(x => !x.IsAdditional(c.Options.AdditionalFiles)))
                {
                    action.Invoke(new(config, c.Compilation, c.Options, c.CancellationToken, c.ReportDiagnostic));
                }
            }
        });
    }

    /// <summary>Registers an action on <see cref="ProjectFileAnalysisContext"/>.</summary>
    public static void RegisterResourceFileAction(this AnalysisContext context, Action<ResourceFileAnalysisContext> action)
    {
        context.RegisterAdditionalFileAction(c =>
        {
            if (ProjectFiles.Global.UpdateResourceFile(c) is { IsXml: true } resource)
            {
                action.Invoke(new(resource, c.Compilation, c.Options, c.CancellationToken, c.ReportDiagnostic));
            }
        });

        // Fallback for detecting files that not have been added as additional files.
        context.RegisterCompilationAction(c =>
        {
            if (ProjectFiles.Global.UpdateMsBuildProject(c) is { } msbuild)
            {
                foreach (var file in msbuild.Path.Directory.Files("**/*.resx") ?? [])
                {
                    if (ProjectFiles.Global.UpdateResourceFile(file) is { IsXml: true } resource
                        && !resource.IsAdditional(c.Options.AdditionalFiles))
                    {
                        action.Invoke(new(resource, c.Compilation, c.Options, c.CancellationToken, c.ReportDiagnostic));
                    }
                }
            }
        });
    }
}
