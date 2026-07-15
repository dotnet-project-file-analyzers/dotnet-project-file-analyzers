namespace DotNetProjectFile.Analyzers;

/// <summary>Extensions to help registering <see cref="ProjectFileAnalyzer{T}"/>'s.</summary>
internal static class AnalyzerRegistry
{
    extension(AnalysisContext context)
    {
        /// <summary>Registers an register on <see cref="ProjectFileAnalysisContext"/>.</summary>
        public void RegisterProjectFileAction(Action<ProjectFileAnalysisContext> register)
        {
            context.RegisterAdditionalFileAction(c =>
            {
                if (ProjectFiles.Global.UpdateMsBuildProject(c) is { IsLegacy: false } msbuild)
                {
                    register(new(msbuild, c.Compilation, c.Options, c.CancellationToken, c.ReportDiagnostic));
                }
            });

            // Fallback for detecting files that not have been added as additional files.
            context.RegisterCompilationAction(c =>
            {
                if (ProjectFiles.Global.UpdateMsBuildProject(c) is { IsLegacy: false } entry)
                {
                    foreach (var msbuild in entry.ImportsAndSelf().Where(x => !x.IsAdditional(c.Options.AdditionalFiles)))
                    {
                        register(new(msbuild, c.Compilation, c.Options, c.CancellationToken, c.ReportDiagnostic));
                    }
                }
            });
        }

        /// <summary>Registers an register on <see cref="ProjectFileAnalysisContext"/>.</summary>
        public void RegisterEditorConfigFileAction(Action<IniFileAnalysisContext> register)
        {
            context.RegisterAdditionalFileAction(c =>
            {
                if (ProjectFiles.Global.UpdateIniFile(c) is { } ini)
                {
                    register(new(ini, c.Compilation, c.Options, c.CancellationToken, c.ReportDiagnostic));
                }
            });

            // Fallback for detecting files that not have been added as additional files.
            context.RegisterCompilationAction(c =>
            {
                if (ProjectFiles.Global.UpdateMsBuildProject(c) is { } msbuild)
                {
                    foreach (var config in msbuild.EditorConfigs().Where(x => !x.IsAdditional(c.Options.AdditionalFiles)))
                    {
                        register(new(config, c.Compilation, c.Options, c.CancellationToken, c.ReportDiagnostic));
                    }
                }
            });
        }

        /// <summary>Registers an register on <see cref="NuGetConfigFileAnalysisContext"/>.</summary>
        public void RegisterNuGetConfigFileAction(Action<NuGetConfigFileAnalysisContext> register)
            => context.RegisterAdditionalFileAction(c =>
            {
                if (ProjectFiles.Global.UpdateNugetConfigFile(c) is { } config)
                {
                    register(new(config, c.Compilation, c.Options, c.CancellationToken, c.ReportDiagnostic));
                }
            });

        /// <summary>Registers an register on <see cref="ProjectFileAnalysisContext"/>.</summary>
        public void RegisterResourceFileAction(Action<ResourceFileAnalysisContext> register)
        {
            context.RegisterAdditionalFileAction(c =>
            {
                if (ProjectFiles.Global.UpdateResourceFile(c) is { IsXml: true } resource)
                {
                    register(new(resource, c.Compilation, c.Options, c.CancellationToken, c.ReportDiagnostic));
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
                            register(new(resource, c.Compilation, c.Options, c.CancellationToken, c.ReportDiagnostic));
                        }
                    }
                }
            });
        }

        /// <summary>Registers an register on <see cref="SolutionFileAnalysisContext"/>.</summary>
        public void RegisterSolutionFileAction(Action<SolutionFileAnalysisContext> register)
            => context.RegisterAdditionalFileAction(c =>
            {
                if (ProjectFiles.Global.UpdateSolutionFile(c) is { } solution)
                {
                    register(new(solution, c.Compilation, c.Options, c.CancellationToken, c.ReportDiagnostic));
                }
            });
    }
}
