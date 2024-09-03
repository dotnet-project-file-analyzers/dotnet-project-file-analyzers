using System.Collections.Concurrent;
using System.IO;

namespace DotNetProjectFile.MsBuild;

public sealed class Projects(string language)
{
    private readonly object locker = new();
    private readonly ConcurrentDictionary<IOFile, AdditionalText> AdditionalTexts = [];
    private readonly ConcurrentDictionary<IOFile, Project> Resolved = [];

    public string Language { get; } = language;

    public MsBuildProject? EntryPoint(CompilationAnalysisContext context)
        => EntryPoint(context.Compilation);

    public MsBuildProject? EntryPoint(AdditionalFileAnalysisContext context)
        => EntryPoint(context.Compilation);

    public MsBuildProject? EntryPoint(Compilation compilation)
    {
        if (compilation.Assembly is { } assembly
            && (EntryPointFromAdditionTexts(assembly.Name) ?? EntryPointFromAssembly(assembly)) is { } entryPoint)
        {
            if (entryPoint.DirectoryBuildProps is null
                && DirectoryProps(compilation.Assembly, "Directory.Build.props") is { } build)
            {
                foreach (var import in entryPoint.ImportsAndSelf().TakeWhile(t => t.FileType != ProjectFileType.DirectoryBuild))
                {
                    import.DirectoryBuildProps = build;
                }
            }
            if (entryPoint.DirectoryPackagesProps is null
                && DirectoryProps(compilation.Assembly, "Directory.Packages.props") is { } package)
            {
                foreach (var import in entryPoint.ImportsAndSelf().TakeWhile(t => t.FileType != ProjectFileType.DirectoryPackages))
                {
                    import.DirectoryPackagesProps = package;
                }
            }
            return entryPoint;
        }
        return null;
    }

    public MsBuildProject? TryResolve(IOFile location)
    {
        lock (locker)
        {
            if (Resolved.TryGetValue(location, out var project))
            {
                return project;
            }
            else if (AdditionalTexts.TryGetValue(location, out var additional)
                && IsProject(location))
            {
                project = Project.Load(additional, this);
                Resolved[location] = project;
                return project;
            }
            else if (IsProject(location))
            {
                project = Project.Load(location, this);
                Resolved[location] = project;
                return project;
            }
            else
            {
                return null;
            }
        }
    }

    private MsBuildProject? DirectoryProps(IAssemblySymbol assembly, string fileName)
       => GetAncestorDirectories(assembly)
           .Select(dir => dir.File(fileName))
           .FirstOrDefault(f => f.Exists) is { } location
       && TryResolve(IOFile.Parse(location.FullName)) is { } props
           ? props
           : null;

    private MsBuildProject? EntryPointFromAdditionTexts(string name)
        => AdditionalTexts.Keys
            .Where(IsProject)
            .Where(l => HasName(l, name))
            .Select(TryResolve)
            .ToArray() is { Length: 1 } projects
                ? projects[0]
                : null;

    private MsBuildProject? EntryPointFromAssembly(IAssemblySymbol assembly)
        => GetAncestorDirectories(assembly).SelectMany(d => d.EnumerateFiles())
            .Select(f => IOFile.Parse(f.FullName))
            .Where(IsProject)
            .Where(l => HasName(l, assembly.Name))
            .Select(TryResolve)
            .ToArray() is { Length: 1 } projects
                ? projects[0]
                : null;

    private static IEnumerable<DirectoryInfo> GetAncestorDirectories(IAssemblySymbol assembly)
      => assembly.Locations
          .Select(l => l.SourceTree?.FilePath)
          .Where(path => path is { })
          .Select(path => new FileInfo(path))
          .SelectMany(file => file.GetAncestors())
          .Distinct(FileSystem.DirectoryComparer)
          .Where(f => f.Exists);

    private bool IsProject(IOFile location)
        => location.Exists
        && IsSupportedExtension(location.Extension);

    private bool IsSupportedExtension(string extension)
        => string.Equals(extension, ".props", StringComparison.OrdinalIgnoreCase)
        || Language switch
        {
            LanguageNames.CSharp => string.Equals(extension, ".csproj", StringComparison.OrdinalIgnoreCase),
            LanguageNames.VisualBasic => string.Equals(extension, ".vbproj", StringComparison.OrdinalIgnoreCase),
            _ => false,
        };

    private static bool HasName(IOFile file, string? name)
        => string.Equals(file.NameWithoutExtension, name, StringComparison.OrdinalIgnoreCase);

    public static Projects Init(CompilationAnalysisContext context)
        => Cache.Get(context.Compilation, () => New(context));

    public static Projects Init(AdditionalFileAnalysisContext context)
        => Cache.Get(context.Compilation, () => New(context));

    private static Projects New(CompilationAnalysisContext context)
        => New(context.Compilation, context.Options);

    private static Projects New(AdditionalFileAnalysisContext context)
        => New(context.Compilation, context.Options);

    private static Projects New(Compilation compilation, AnalyzerOptions options)
    {
        var projects = new Projects(compilation.Options.Language);

        foreach (var additional in options.AdditionalFiles)
        {
            var location = IOFile.Parse(additional.Path);
            if (projects.IsProject(location))
            {
                projects.AdditionalTexts[location] = additional;
            }
        }
        return projects;
    }

    private static readonly CompilationCache<Projects> Cache = new();
}
