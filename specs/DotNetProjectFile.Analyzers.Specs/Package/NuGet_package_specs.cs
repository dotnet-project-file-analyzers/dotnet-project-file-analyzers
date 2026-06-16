using System.IO;
using System.IO.Compression;

namespace Specs.NuGet_package_specs;

#if !DEBUG
[Explicit]
#endif
public class Contains
{
    [TestCaseSource(nameof(Packages))]
    public void Entries(FileInfo file)
    {
        using var archive = ZipFile.OpenRead(file.FullName);

        var entries = archive.Entries.Select(e => e.FullName).Where(e => !e.EndsWith(".psmdcp"));

        foreach (var e in entries)
            Console.WriteLine(e);


        entries.Should().BeEquivalentTo(
            // NuSpec
            "DotNetProjectFile.Analyzers.nuspec",
            "logo_128x128.png",
            "README.md",

            // Analyzer       
            "analyzers/DotNetProjectFile.Analyzers.dll",

            // Build props and targets
            "build/AdditionalFiles.props",
            "build/AdditionalFiles.Sdk.props",
            "build/CompilerVisible.props",
            "build/DotNetProjectFile.Analyzers.props",
            "build/DotNetProjectFile.Analyzers.Sdk.props",
            "build/DotNetProjectFile.Analyzers.targets",
            "build/None.Sdk.props",
            
            // tools
            "tools/install.ps1",
            "tools/uninstall.ps1",

            // Added by NuGet
            "[Content_Types].xml",
            "_rels/.rels", 
            "_manifest/spdx_2.2/manifest.spdx.json",
            "_manifest/spdx_2.2/manifest.spdx.json.sha256");
    }


    private static IEnumerable<FileInfo> Packages => Root.EnumerateFiles("*.nupkg");

    private static readonly DirectoryInfo Root = new("../../../../../src/DotNetProjectFile.Analyzers/bin/Release");
}
