using System.IO;

namespace DotNetProjectFile.MsBuild;

public sealed class Projects(string language)
{
    private readonly object locker = new();
    private readonly Dictionary<FileInfo, AdditionalText> AdditionalTexts = new(FileSystemEqualityComparer.File);
    private readonly Dictionary<FileInfo, Project> Resolved = new(FileSystemEqualityComparer.File);

    public string Language { get; } = language;

    public Project? EntryPoint(CompilationAnalysisContext context)
    {
        if (context.Compilation.Assembly is { } assembly
            && (EntryPointFromAdditionTexts(assembly.Name) ?? EntryPointFromAssembly(assembly)) is { } entryPoint)
        {
            if (entryPoint.DirectoryBuildProps is null
                && DirectoryBuildProps(context.Compilation.Assembly) is { } props)
            {
                entryPoint.DirectoryBuildProps = props;
            }
            return entryPoint;
        }
        return null;
    }

    public Project? TryResolve(FileInfo location, bool isProject)
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
                project = Project.Load(additional, this, isProject);
                Resolved[location] = project;
                return project;
            }
            else if (IsProject(location))
            {
                project = Project.Load(location, this, isProject);
                Resolved[location] = project;
                return project;
            }
            else
            {
                return null;
            }
        }
    }

    private Project? DirectoryBuildProps(IAssemblySymbol assembly)
        => GetAncestorDirectories(assembly)
            .Select(dir => dir.File("Directory.Build.props"))
            .FirstOrDefault(f => f.Exists) is { } location
        && TryResolve(location, false) is { } props
            ? props
            : null;

    private Project? EntryPointFromAdditionTexts(string name)
    {
        var projects = AdditionalTexts.Keys.Where(IsProject)
            .Where(l => HasName(l, name))
            .Select(f => TryResolve(f, isProject: true))
            .ToArray();

        return projects.Length == 1 ? projects[0] : null;
    }

    private Project? EntryPointFromAssembly(IAssemblySymbol assembly)
    {
        var directories = GetAncestorDirectories(assembly);

        var projects = directories.SelectMany(d => d.EnumerateFiles())
            .Where(IsProject)
            .Where(l => HasName(l, assembly.Name))
            .Select(f => TryResolve(f, isProject: true))
            .ToArray();

        return projects.Length == 1 ? projects[0] : null;
    }

    private static IEnumerable<DirectoryInfo> GetAncestorDirectories(IAssemblySymbol assembly)
      => assembly.Locations
          .Select(l => l.SourceTree?.FilePath)
          .Where(path => path is { })
          .Select(path => new FileInfo(path))
          .SelectMany(file => file.GetAncestors())
          .Distinct(FileSystemEqualityComparer.Directory)
          .Where(f => f.Exists);

    private bool IsProject(FileInfo location)
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

    private static bool HasName(FileInfo file, string? name)
        => string.Equals(Path.GetFileNameWithoutExtension(file.FullName), name, StringComparison.OrdinalIgnoreCase);

    public static Projects Init(CompilationAnalysisContext context)
        => Cache.Get(context.Compilation, () => New(context));

    private static Projects New(CompilationAnalysisContext context)
    {
        var projects = new Projects(context.Compilation.Options.Language);

        foreach (var additional in context.Options.AdditionalFiles)
        {
            var location = new FileInfo(additional.Path);
            if (projects.IsProject(location))
            {
                projects.AdditionalTexts[location] = additional;
            }
        }
        return projects;
    }

    private static readonly CompilationCache<Projects> Cache = new();
}
