using DotNetProjectFile.IO;
using System.IO;
using System.Xml.Linq;

namespace DotNetProjectFile.Xml;

public sealed class Project : Node
{
    public Project(XElement element) : base(element) { }

    public Nodes<PropertyGroup> PropertyGroups => GetChildren<PropertyGroup>();

    public Nodes<ItemGroup> ItemGroups => GetChildren<ItemGroup>();

    public static Project Load(FileInfo file)
        => new (XElement.Load(file.OpenRead(), LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo));
    
    public static Project? Create(Compilation compilation)
    {
        if (compilation.SourceModule.ContainingAssembly is { } assembly)
        {
            var directories = assembly.Locations.Select(l => l.SourceTree?.FilePath)
                .Where(p => p is { })
                .Select(p => new FileInfo(p).Directory)
                .Distinct(DirectoryEqualityComparer.Instance)
                .Where(f => f.Exists)
                .ToArray();

            var project = directories.Select(d => GetFile(d, $"{assembly.Name}.??proj"))
                .FirstOrDefault(f => f is { });

            if (project is { })
            {
                return Load(project);
            }
        }
        return null;
    }
    private static FileInfo? GetFile(DirectoryInfo dir, string fileName)
    {
        FileInfo? file;
        try
        {
            file = dir.GetFiles(fileName).FirstOrDefault();
        }
        catch
        {
            return null;
        }
        return file is null && dir.Parent is { }
            ? GetFile(dir.Parent, fileName)
            : file;
    }
}
