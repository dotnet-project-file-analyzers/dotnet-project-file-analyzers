using DotNetProjectFile.Git;
using DotNetProjectFile.Ini;
using DotNetProjectFile.Resx;
using DotNetProjectFile.Slnx;

namespace DotNetProjectFile.CodeAnalysis;

public sealed partial class ProjectFiles
{
    public static readonly ProjectFiles Global = new();

    private readonly FileCache<GitIgnoreFile> GitIgnoredFiles = new();
    private readonly FileCache<IniFile> IniFiles = new();
    private readonly FileCache<MsBuildProject> MsBuildProjects = new();
    private readonly FileCache<NuGet.Configuration.NuGetConfigFile> NuGetConfigFiles = new();
    private readonly FileCache<Resource> ResourceFiles = new();
    private readonly FileCache<SolutionFile> SolutionFiles = new();

    public GitIgnoreFile? GitIgnoreFile(IOFile file)
        => GitIgnoredFiles.TryGetOrUpdate(file, Create_GitIgnoreFile);

    public IniFile? IniFile(IOFile file)
        => IniFiles.TryGetOrUpdate(file, Create_IniFile);

    public MsBuildProject? MsBuildProject(IOFile file)
        => MsBuildProjects.TryGetOrUpdate(file, Create_MsBuildProject);

    public MsBuildProject? MsBuildProject(AdditionalText text)
        => AnalyzerTypes.MsBuild(text.Location) is { } type
        ? MsBuildProjects.TryGetOrUpdate(text.Location, _ => MsBuild.MsBuildProject.Load(type, text, Global))
        : null;

    public Resource? ResourceFile(IOFile file)
        => ResourceFiles.TryGetOrUpdate(file, Create_ResourceFile);

    public MsBuildProject? UpdateMsBuildProject(CompilationAnalysisContext context)
    {
        if (GetBuildProjectFile(context) is not { Length: > 0 } file)
        {
            return null;
        }

        // If it is amongst the additional files, do not look further.
        return context.Options.AdditionalFiles
            .Select(a => a.Location)
            .FirstOrDefault(f => f.Name.IsMatch(file)) is { HasValue: true } additional

            ? MsBuildProject(additional)

            : context.Compilation?.Assembly?.Locations
                .Select(l => IOFile.Parse(l.SourceTree?.FilePath))
                .Where(file => file.HasValue)
                .SelectMany(file => file.Directory.AncestorsAndSelf())
                .Distinct()
                .Select(dir => MsBuildProject(dir.File(file)))
                .OfType<MsBuildProject>()
                .FirstOrDefault();

        static string? GetBuildProjectFile(CompilationAnalysisContext context)
        {
            if (new MsBuildProps(context.Options).MSBuildProjectFile is { Length: > 0 } file)
            {
                return file;
            }

            var extension = Language.Parse(context.Compilation.Language).ProjectFileExtension;

            return extension is null || context.Compilation.AssemblyName is not { Length: > 0 } name
                ? null
                : name + extension;
        }
    }

    public AnalyzerFileInfo<MsBuildProject>? UpdateMsBuildProject(AdditionalFileAnalysisContext context)
        => context.AnyOf(
            AnalyzerTypes.MsBuild,
            AnalyzerType.MSBuildProject,
            AnalyzerType.MSBuildProps,
            AnalyzerType.DirectoryBuildProps,
            AnalyzerType.DirectoryBuildTargets,
            AnalyzerType.DirectoryPackagesProps,
            AnalyzerType.SDK) is { } type

        && MsBuildProjects.TryGetOrUpdate(context, _ => MsBuild.MsBuildProject.Load(type, context.AdditionalFile, this)) is { } file
            ? new(file, type)
            : null;

    public AnalyzerFileInfo<IniFile>? UpdateIniFile(AdditionalFileAnalysisContext context)
        => context.AnyOf(
            AnalyzerTypes.Ini,
            AnalyzerType.EditorConfig,
            AnalyzerType.GlobalConfig) is { } type

        && IniFiles.TryGetOrUpdate(context, _ => Ini.IniFile.Load(context.AdditionalFile)) is { } file
            ? new(file, type)
            : null;

    public AnalyzerFileInfo<Resource>? UpdateResourceFile(AdditionalFileAnalysisContext context)
         => context.AnyOf(
            path => path.Extension.IsMatch(".resx") ? AnalyzerType.RESX : null,
            AnalyzerType.RESX) is { } type

        && ResourceFiles.TryGetOrUpdate(context, _ => Resource.Load(context.AdditionalFile, this)) is { } file
            ? new(file, type)
            : null;

    public AnalyzerFileInfo<NuGet.Configuration.NuGetConfigFile>? UpdateNugetConfigFile(AdditionalFileAnalysisContext context)
         => context.AnyOf(
            path => path.Name.IsMatch("NuGet.config") ? AnalyzerType.NuGetConfig : null,
            AnalyzerType.NuGetConfig) is { } type

        && NuGetConfigFiles.TryGetOrUpdate(context, _ => NuGet.Configuration.NuGetConfigFile.Load(context.AdditionalFile)) is { } file
            ? new(file, type)
            : null;

    public AnalyzerFileInfo<SolutionFile>? UpdateSolutionFile(AdditionalFileAnalysisContext context)
          => context.AnyOf(
            path => path.Extension.IsMatch(".slnx") ? AnalyzerType.SLNX : null,
            AnalyzerType.SLNX) is { } type

        && SolutionFiles.TryGetOrUpdate(context, _ => Slnx.SolutionFile.Load(context.AdditionalFile, this)) is { } file
            ? new(file, type)
            : null;

    private static GitIgnoreFile Create_GitIgnoreFile(IOFile file)
        => Git.GitIgnoreFile.Load(file)!;

    private static IniFile Create_IniFile(IOFile file)
        => Ini.IniFile.Load(file)!;

    private MsBuildProject? Create_MsBuildProject(IOFile file)
       => MsBuild.MsBuildProject.Load(file, this);


    private Resource Create_ResourceFile(IOFile file)
        => Resource.Load(file, this);
}
