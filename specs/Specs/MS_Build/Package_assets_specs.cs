using Buildalyzer.Environment;
using System.IO;
using static Specs.TestTools.TestPath;
using Meta = Specs.TestTools.ProjectItem.Meta;
using ProjectItem = Specs.TestTools.ProjectItem;

namespace MS_Build.Package_assets_specs;

[NonParallelizable] // these tests all build the shared CompliantCSharpPackage fixture (BuildalyzerContext
// wipes its bin/obj), so running them in parallel races the pack and intermittently loses the nupkg.
public class Builds
{
    [Test, Ignore("HaveItems does not support dupplicate keys, and as this test is about to change, we ignore this for now.")]
    public void With_defaults()
    {
        using var ctx = BuildalyzerContext.ForProject("CompliantCSharpPackage/CompliantCSharpPackage.csproj");

        var result = ctx.Analyzer.Build().Results.Single();

        result.Should().HaveProperties(new()
            {
                ["SonarQubeIntegration"] = "true"
            })

            .And.HaveCompilerVisibleProperties(
                "Configuration",
                "EnableNETAnalyzers",
                "IsPackable",
                "IsTestProject",
                "LangVersion",
                "ManagePackageVersionsCentrally",
                "MSBuildProjectFile",
                "MSBuildThisFileDirectory",
                "NETCoreSdkVersion",
                "PackageLicenseExpression",
                "Platform",
                "RestoreLockedMode",
                "SolutionDir")

            .And.HaveAdditionalFiles(
                new()
                {
                    ItemSpec = Full("CompliantCSharpPackage/CompliantCSharpPackage.csproj"),
                    Metadata = new Meta
                    {
                        AnalyzerType = "MSBuildProject",
                        Visible = "false",
                    },
                },
                new() { ItemSpec = "compliant-package.slnx" },
                new() { ItemSpec = "Messages.resx" })

            .And.HaveItems(
                "Content",
                new ProjectItem
                {
                    ItemSpec = "../../design/logo_128x128.png",
                    Metadata = new Meta
                    {
                        Link = "logo_128x128.png",
                    },
                });
    }

    [Test]
    public void Package()
    {
        using var ctx = BuildalyzerContext.ForProject("CompliantCSharpPackage/CompliantCSharpPackage.csproj");

        var options = new EnvironmentOptions() { DesignTime = false };
        options.Arguments.Add("-p:GeneratePackageOnBuild=true");
        options.Arguments.Add("-p:Configuration=RELEASE");

        var result = ctx.Analyzer.Build(options).Results.Single();
        result.Succeeded.Should().BeTrue("the package build must succeed before its output can be inspected");

        var artifacts = new DirectoryInfo(Path.Combine(ctx.Location.Directory!.FullName, "../artifacts"));
        var packages = artifacts.Exists
            ? artifacts.EnumerateFiles("*.nupkg", SearchOption.AllDirectories).ToArray()
            : Array.Empty<FileInfo>();
        packages.Should().ContainSingle("a single nupkg should be produced under {0}", artifacts.FullName);
        var package = packages.Single();

        var payload = Nupkg.Read(package).Where(e => !Ignore(e));

        payload.Should().BeEquivalentTo(
            "[Content_Types].xml",
            "README.md",
            "logo_128x128.png",
            "build/CompliantCSharpPackage.props",
            "build/CompliantCSharpPackage.targets",
            "content/logo_128x128.png",
            "contentFiles/any/net10.0/logo_128x128.png",
            "lib/net10.0/CompliantCSharpPackage.dll",
            "lib/net10.0/CompliantCSharpPackage.xml");

        // NuGet and SBOM scaffolding filtered out so the assertion pins the actual payload.
        static bool Ignore(string e)
            => e.StartsWith("_rels/", StringComparison.Ordinal)
            || e.StartsWith("_manifest/", StringComparison.Ordinal)
            || e.EndsWith(".nuspec", StringComparison.Ordinal);
    }
}
