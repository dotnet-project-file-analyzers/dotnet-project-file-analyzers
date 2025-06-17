using DotNetProjectFile.Git;
using DotNetProjectFile.Ini;
using DotNetProjectFile.Resx;
using DotNetProjectFile.Slnx;

namespace DotNetProjectFile.CodeAnalysis;

public sealed partial class ProjectFiles
{
    public static readonly ProjectFiles Global = new();

    private readonly FileCache<GitIgnoreSyntax> GitIgnoredFiles = new();
    private readonly FileCache<IniFile> IniFiles = new();
    private readonly FileCache<MsBuildProject> MsBuildProjects = new();
    private readonly FileCache<Resource> ResourceFiles = new();
    private readonly FileCache<Solution> SolutionFiles = new();

    public GitIgnoreSyntax? GitIgnoreFile(IOFile file)
        => GitIgnoredFiles.TryGetOrUpdate(file, Create_GitIgnoreFile);

    public IniFile? IniFile(IOFile file)
        => IniFiles.TryGetOrUpdate(file, Create_IniFile);

    public MsBuildProject? MsBuildProject(IOFile file)
        => MsBuildProjects.TryGetOrUpdate(file, Create_MsBuildProject);

    public MsBuildProject? MsBuildProject(AdditionalText text)
    {
        var path = IOFile.Parse(text.Path);
        if (path.GetProjectFileType() is ProjectFileType.None) return null;

        return MsBuildProjects.TryGetOrUpdate(path, _ => MsBuild.Project.Load(text, Global));
    }

    public Resource? ResourceFile(IOFile file)
        => ResourceFiles.TryGetOrUpdate(file, Create_ResourceFile);

    public Solution? SolutionFile(AdditionalText text)
    {
        var path = IOFile.Parse(text.Path);
        return SolutionFiles.TryGetOrUpdate(path, _ => Solution.Load(text, Global));
    }

    public Solution? SolutionFile(IOFile file)
        => SolutionFiles.TryGetOrUpdate(file, _ => Solution.Load(file, Global));

    public MsBuildProject? UpdateMsBuildProject(CompilationAnalysisContext context)
    {
        if (GetBuildProjectFile(context) is not { Length: > 0 } file)
        {
            return null;
        }

        // If it is amongst the additional files, do not look further.
        if (context.Options.AdditionalFiles
            .Select(a => IOFile.Parse(a.Path))
            .FirstOrDefault(f => f.Name.IsMatch(file)) is { HasValue: true } additional)
        {
            return MsBuildProject(additional);
        }

        return context.Compilation?.Assembly?.Locations
            .Select(l => IOFile.Parse(l.SourceTree?.FilePath))
            .Where(file => file.HasValue)
            .SelectMany(file => file.Directory.AncestorsAndSelf())
            .Distinct()
            .Select(dir => MsBuildProject(dir.File(file)))
            .OfType<MsBuildProject>()
            .FirstOrDefault();

        static string? GetBuildProjectFile(CompilationAnalysisContext context)
        {
            if (context.Options.GetMsBuildProperty("MSBuildProjectFile") is { Length: > 0 } file)
            {
                return file;
            }

            var extension = context.Compilation.Language switch
            {
                LanguageNames.CSharp => ".csproj",
                LanguageNames.VisualBasic => ".vbproj",
                _ => null,
            };

            return extension is null || context.Compilation.AssemblyName is not { Length: > 0 } name
                ? null
                : name + extension;
        }
    }

    public IniFile? UpdateIniFile(AdditionalFileAnalysisContext context)
    {
        var file = IOFile.Parse(context.AdditionalFile.Path);
        return Is.Ini(file)
            ? IniFiles.TryGetOrUpdate(file, _ => new IniFile(IniFileSyntax.Parse(Syntax.SyntaxTree.From(context.AdditionalFile))))
            : null;
    }

    public MsBuildProject? UpdateMsBuildProject(AdditionalFileAnalysisContext context)
    {
        var file = IOFile.Parse(context.AdditionalFile.Path);
        return Is.MsBuild(file)
            ? MsBuildProjects.TryGetOrUpdate(file, _ => MsBuild.Project.Load(context.AdditionalFile, this))
            : null;
    }

    public Resource? UpdateResourceFile(AdditionalFileAnalysisContext context)
    {
        var file = IOFile.Parse(context.AdditionalFile.Path);
        return Is.Resource(file)
            ? ResourceFiles.TryGetOrUpdate(file, _ => Resource.Load(context.AdditionalFile, this))
            : null;
    }

    public Solution? UpdateSolutionFile(AdditionalFileAnalysisContext context)
    {
        var file = IOFile.Parse(context.AdditionalFile.Path);
        return Is.Solution(file)
            ? SolutionFiles.TryGetOrUpdate(file, _ => Solution.Load(context.AdditionalFile, this))
            : null;
    }

    public Resource? UpdateResourceFile(IOFile file)
         => Is.Resource(file)
            ? ResourceFiles.TryGetOrUpdate(file, _ => Resource.Load(file, this))
            : null;

    private static GitIgnoreSyntax Create_GitIgnoreFile(IOFile file)
        => GitIgnoreSyntax.Parse(Syntax.SyntaxTree.Load(file.OpenRead()));

    private static IniFile Create_IniFile(IOFile file)
        => new(IniFileSyntax.Parse(Syntax.SyntaxTree.Load(file.OpenRead())));

    private MsBuildProject Create_MsBuildProject(IOFile file)
       => MsBuild.Project.Load(file, this);

    private Resource Create_ResourceFile(IOFile file)
        => Resource.Load(file, this);

    private static class Is
    {
        public static bool Ini(IOFile file)
            => file.Extension.IsMatch(".ini")
            || file.Extension.IsMatch(".editorconfig")
            || file.Extension.IsMatch(".globalconfig");

        public static bool MsBuild(IOFile file)
            => file.Extension.IsMatch(".csproj")
            || file.Extension.IsMatch(".props")
            || file.Extension.IsMatch(".vbproj");

        public static bool Resource(IOFile file) => file.Extension.IsMatch(".resx");

        public static bool Solution(IOFile file) => file.Extension.IsMatch(".slnx");
    }
}
