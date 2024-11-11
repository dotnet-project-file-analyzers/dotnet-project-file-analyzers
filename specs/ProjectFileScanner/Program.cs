using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ProjectFileScanner;

public static class Program
{
    static void Main(string[] args)
    {
        var paths = args[0].Split(';', StringSplitOptions.TrimEntries);

        var roots = paths.Select(p => new DirectoryInfo(p)).ToArray();

        var nodes = new Dictionary<string, int>();

        foreach (var file in roots
            .SelectMany(r => r.EnumerateFiles("*.*", SearchOption.AllDirectories))
            .Where(IsProjectFile))
        {
            AddNodes(file, nodes);
        }

        using var writer = new StreamWriter("C://TEMP/nodes.txt");

        foreach (var kvp in nodes.OrderByDescending(kvp => kvp.Value))
        {
            writer.WriteLine($"{kvp.Key,5} {kvp.Value}");
        }
    }

    private static void AddNodes(FileInfo file, Dictionary<string, int> nodes)
    {
        try
        {
            using var stream = file.OpenRead();
            var proj = XDocument.Load(stream);
            if (proj.Root?.Name.LocalName != "Project") return;

            if(proj.Root.Name.NamespaceName != "http://schemas.microsoft.com/developer/msbuild/2003")
            {
                return;    
            }

            foreach (var node in proj.Descendants())
            {
                var path = string.Join('/', node.AncestorsAndSelf().Select(p => p.Name.LocalName).Reverse());

                nodes.TryAdd(path, 0);
                nodes[path]++;
            }
        }
        catch { }
    }

    private static bool IsProjectFile(FileInfo info) => Extenions.Contains(info.Extension.ToUpperInvariant());

        private static readonly string[] Extenions = [".CSPROJ", ".PROPS", ".VBPROJ"];
}
