using System.IO;

namespace Specs.NuGet_package_specs;

#if !DEBUG
[Explicit]
#endif
public class Contains
{
    [TestCaseSource(nameof(Packages))]
    public void Entries(FileInfo file)
    {
        var entries = Nupkg.Read(file).Order();

        foreach (var e in entries)
            Console.WriteLine(e);

        entries.Should().BeEquivalentTo(
            "_manifest/spdx_2.2/manifest.spdx.json",
            "_manifest/spdx_2.2/manifest.spdx.json.sha256",
            "_rels/.rels",
            "[Content_Types].xml",
            "analyzers/DotNetProjectFile.Analyzers.dll",
            "build/AdditionalFiles.Sdk.props",
            "build/CompilerVisible.props", 
            "build/AdditionalFiles.targets",
            "build/DotNetProjectFile.Analyzers.props",
            "build/DotNetProjectFile.Analyzers.Sdk.props",
            "build/DotNetProjectFile.Analyzers.targets",
            "build/None.Sdk.props",
            "DotNetProjectFile.Analyzers.nuspec",
            "logo_128x128.png",
            "README.md",
            "tools/install.ps1",
            "tools/uninstall.ps1");
    }


    private static IEnumerable<FileInfo> Packages => Root.EnumerateFiles("*.nupkg");

    private static readonly DirectoryInfo Root = new("../../../../../src/DotNetProjectFile.Analyzers/bin/Release");
}
