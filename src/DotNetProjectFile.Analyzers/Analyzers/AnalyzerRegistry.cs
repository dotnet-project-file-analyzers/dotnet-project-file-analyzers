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
                if (ProjectFiles.Global.UpdateMsBuildProject(c) is { File.IsLegacy: false } msbuild)
                {
                    register(new(msbuild.File, msbuild.Type, c.Compilation, c.Options, c.CancellationToken, c.ReportDiagnostic));
                }
            });

            // Fallback for detecting files that not have been added as additional files.
            context.RegisterCompilationAction(c =>
            {
                if (ProjectFiles.Global.UpdateMsBuildProject(c) is { IsLegacy: false } entry)
                {
                    foreach (var msbuild in entry.ImportsAndSelf().Where(x => !x.IsAdditional(c.Options.AdditionalFiles)))
                    {
                        register(new(msbuild, AnalyzerTypes.MsBuild(msbuild.Path) ?? AnalyzerType.DirectoryBuildProps, c.Compilation, c.Options, c.CancellationToken, c.ReportDiagnostic));
                    }
                }
            });
        }

        /// <summary>Registers an register on <see cref="ProjectFileAnalysisContext"/>.</summary>
        public void RegisterEditorConfigFileAction(Action<IniFileAnalysisContext> register)
            => context.RegisterAdditionalFileAction(c =>
            {
                if (ProjectFiles.Global.UpdateIniFile(c) is { } ini)
                {
                    register(new(ini.File, ini.Type, c.Compilation, c.Options, c.CancellationToken, c.ReportDiagnostic));
                }
            });

        /// <summary>Registers an register on <see cref="NuGetConfigFileAnalysisContext"/>.</summary>
        public void RegisterNuGetConfigFileAction(Action<NuGetConfigFileAnalysisContext> register)
            => context.RegisterAdditionalFileAction(c =>
            {
                if (ProjectFiles.Global.UpdateNugetConfigFile(c) is { } config)
                {
                    register(new(config.File, config.Type, c.Compilation, c.Options, c.CancellationToken, c.ReportDiagnostic));
                }
            });

        /// <summary>Registers an register on <see cref="ProjectFileAnalysisContext"/>.</summary>
        public void RegisterResourceFileAction(Action<ResourceFileAnalysisContext> register)
            => context.RegisterAdditionalFileAction(c =>
            {
                if (ProjectFiles.Global.UpdateResourceFile(c) is { File.IsXml: true } resource)
                {
                    register(new(resource.File, resource.Type, c.Compilation, c.Options, c.CancellationToken, c.ReportDiagnostic));
                }
            });

        /// <summary>Registers an register on <see cref="SolutionFileAnalysisContext"/>.</summary>
        public void RegisterSolutionFileAction(Action<SolutionFileAnalysisContext> register)
            => context.RegisterAdditionalFileAction(c =>
            {
                if (ProjectFiles.Global.UpdateSolutionFile(c) is { } solution)
                {
                    register(new(solution.File, solution.Type, c.Compilation, c.Options, c.CancellationToken, c.ReportDiagnostic));
                }
            });
    }
}
