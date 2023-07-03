using Microsoft.CodeAnalysis.Diagnostics;
using System.IO;

namespace DotNetProjectFile.IO;

public static class ProjectFileResolver
{
    public static Xml.Project? Resolve(CompilationAnalysisContext context)
    {
        if (context.Compilation.SourceModule.ContainingAssembly is { } assembly)
        {
            var isProject = IsProject(context.Compilation.Options.Language);

            return Resolve(context.Options.AdditionalFiles, assembly.Name, isProject)
                ?? Resolve(assembly.Locations, assembly.Name, isProject);
        }
        return null;
    }

    private static Xml.Project? Resolve(IEnumerable<AdditionalText> texts, string assemblyName, Func<FileInfo, string, bool> isProject)
    {
        var additionals = texts
               .Where(f => isProject(new(f.Path), assemblyName))
               .ToArray();

        return additionals.Length == 1
            ? Xml.Project.Load(additionals[0])
            : null;
    }

    private static Xml.Project? Resolve(IEnumerable<Location> locations, string assemblyName, Func<FileInfo, string, bool> isProject)
    {
        var directories = locations.Select(l => l.SourceTree?.FilePath)
               .Where(path => path is { })
               .Select(path => new FileInfo(path))
               .SelectMany(Ancestors)
               .Distinct(DirectoryEqualityComparer.Instance)
               .Where(f => f.Exists);

        var files = directories.SelectMany(d => d.EnumerateFiles())
            .Where(f => isProject(f, assemblyName))
            .ToArray();

        return files.Length == 1
           ? Xml.Project.Load(files[0])
           : null;
    }

    private static IEnumerable<DirectoryInfo> Ancestors(FileInfo file)
    {
        var current = file.Directory;
        while (current is { })
        {
            yield return current;
            current = current.Parent;
        }
    }

    private static Func<FileInfo, string, bool> IsProject(string language)
        => language switch
        {
            LanguageNames.CSharp => IsCSharpProject,
            LanguageNames.VisualBasic => IsVBProject,
            _ => NoMatch,
        };

    private static bool IsCSharpProject(FileInfo file, string name) => IsProject(file, name, ".csproj");

    private static bool IsVBProject(FileInfo file, string name) => IsProject(file, name, ".vbproj");

    private static bool NoMatch(FileInfo _, string __) => false;

    private static bool IsProject(FileInfo file, string name, string extension)
        => file.Exists
        && string.Equals(file.Extension, extension, StringComparison.OrdinalIgnoreCase)
        && string.Equals(Path.GetFileNameWithoutExtension(file.Name), name, StringComparison.OrdinalIgnoreCase);
}
