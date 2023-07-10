using DotNetProjectFile.IO;
using Microsoft.CodeAnalysis.Diagnostics;
using System.IO;

namespace DotNetProjectFile.MsBuild;

public sealed class Projects
{
    private readonly object locker = new();
    private readonly Dictionary<FileInfo, AdditionalText> AdditionalTexts = new(FileSystemEqualityComparer.File);

    private readonly Dictionary<FileInfo, Project> Resolved = new(FileSystemEqualityComparer.File);

    public Projects(string language) => Language = language;

    public string Language { get; }

    public Project? EntryPoint(IAssemblySymbol? assembly)
        => assembly is { }
        ? EntryPointFromAdditionTexts(assembly.Name) ?? EntryPointFromAssembly(assembly)
        : null;

    public Project? TryResolve(FileInfo location)
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

    private Project? EntryPointFromAdditionTexts(string name)
    {
        var projects = AdditionalTexts.Keys.Where(IsProject)
            .Where(l => HasName(l, name))
            .Select(TryResolve)
            .ToArray();

        return projects.Length == 1 ? projects[0] : null;
    }

    private Project? EntryPointFromAssembly(IAssemblySymbol assembly)
    {
        var directories = assembly.Locations.Select(l => l.SourceTree?.FilePath)
            .Where(path => path is { })
            .Select(path => new FileInfo(path))
            .SelectMany(file => file.GetAncestors())
            .Distinct(FileSystemEqualityComparer.Directory)
            .Where(f => f.Exists);

        var projects = directories.SelectMany(d => d.EnumerateFiles())
            .Where(IsProject)
            .Where(l => HasName(l, assembly.Name))
            .Select(TryResolve)
            .ToArray();

        return projects.Length == 1 ? projects[0] : null;
    }

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
}
