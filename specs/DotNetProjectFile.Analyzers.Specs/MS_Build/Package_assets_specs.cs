using Buildalyzer.Environment;
using System.IO;
using Meta = Specs.TestTools.ProjectItem.Meta;
using ProjectItem = Specs.TestTools.ProjectItem;

namespace MS_Build.Package_assets_specs;

public class Builds
{
#if Is_Windows
    private const char Slash = '\\';
#else
    private const char Slash = '/';
#endif

    [Test]
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

            .And.HaveItems(
                "AdditionalFiles",
                new()
                {
                    ItemSpec = """CompliantCSharpPackage.csproj""",
                    Metadata = new Meta { Visible = "false" },
                },
                new() { ItemSpec = "compliant-package.slnx" },
                new() { ItemSpec = "Messages.resx" })

            .And.HaveItems(
                "Content",
                new()
                {
                    ItemSpec = """../../design/logo_128x128.png""",
                    Metadata = new Meta
                    {
                        Link = "logo_128x128.png",
                    },
                },
                new()
                {
                    ItemSpec = ".editorconfig",
                    Metadata = new Meta
                    {
                        CopyToOutputDirectory = "never",
                        Link = ".editorconfig",
                        Visible = "false",
                        SonarQubeContent = "true",
                    },
                },
                new()
                {
                    ItemSpec = ".globalconfig",
                    Metadata = new Meta
                    {
                        CopyToOutputDirectory = "never",
                        Link = ".globalconfig",
                        Visible = "false",
                        SonarQubeContent = "true",
                    },
                },
                new()
                {
                    ItemSpec = $"""build{Slash}CompliantCSharpPackage.props""",
                    Metadata = new Meta
                    {
                        CopyToOutputDirectory = "never",
                        Link = "build_CompliantCSharpPackage.props",
                        Visible = "false",
                        SonarQubeContent = "true",
                    },
                },
                new()
                {
                    ItemSpec = $"""build{Slash}CompliantCSharpPackage.props""",
                    Metadata = new Meta
                    {
                        CopyToOutputDirectory = "never",
                        Link = "build_CompliantCSharpPackage.props",
                        PackagePath = "/build/",
                        Visible = "false",
                        Pack = "true",
                        SonarQubeContent = "true",
                    },
                },
                new()
                {
                    ItemSpec = $"""build{Slash}CompliantCSharpPackage.targets""",
                    Metadata = new Meta
                    {
                        CopyToOutputDirectory = "never",
                        Link = "build_CompliantCSharpPackage.targets",
                        Visible = "false",
                        SonarQubeContent = "true",
                    },
                },
                new()
                {
                    ItemSpec = $"""build{Slash}CompliantCSharpPackage.targets""",
                    Metadata = new Meta
                    {
                        CopyToOutputDirectory = "never",
                        Link = "build_CompliantCSharpPackage.targets",
                        PackagePath = "/build/",
                        Visible = "false",
                        Pack = "true",
                        SonarQubeContent = "true",
                    },
                },
                new()
                {
                    ItemSpec = "compliant-package.slnx",
                    Metadata = new Meta
                    {
                        CopyToOutputDirectory = "never",
                        Link = "compliant-package.slnx",
                        Visible = "false",
                        SonarQubeContent = "true",
                    },
                },
                new()
                {
                    ItemSpec = """CompliantCSharpPackage.csproj""",
                    Metadata = new Meta
                    {
                        CopyToOutputDirectory = "never",
                        Link = "CompliantCSharpPackage.csproj",
                        Visible = "false",
                        SonarQubeContent = "true",
                    },
                },
                new()
                {
                    ItemSpec = """Messages.resx""",
                    Metadata = new Meta
                    {
                        CopyToOutputDirectory = "never",
                        Link = "Messages.resx",
                        Visible = "false",
                        SonarQubeContent = "true",
                    },
                },
                new()
                {
                    ItemSpec = """packages.lock.json""",
                    Metadata = new Meta
                    {
                        CopyToOutputDirectory = "never",
                        Link = "packages.lock.json",
                        Visible = "false",
                        SonarQubeContent = "true",
                    },
                },
                new()
                {
                    ItemSpec = """README.md""",
                    Metadata = new Meta
                    {
                        CopyToOutputDirectory = "never",
                        Link = "README.md",
                        Visible = "false",
                        SonarQubeContent = "true",
                    },
                },
                new()
                {
                    ItemSpec = """README.md""",
                    Metadata = new Meta
                    {
                        CopyToOutputDirectory = "never",
                        Link = "README.md",
                        PackagePath = "",
                        Pack = "true",
                        Visible = "false",
                        SonarQubeContent = "true",
                    },
                });
    }

    [Test]
#if !DEBUG
    [Explicit(reason: "nupkg can not be resolved at the build server")]
#endif
    public void Package()
    {
        using var ctx = BuildalyzerContext.ForProject("CompliantCSharpPackage/CompliantCSharpPackage.csproj");
        
        var options = new EnvironmentOptions() { DesignTime = false };
        options.Arguments.Add("-p:GeneratePackageOnBuild=true");
        options.Arguments.Add("-p:Configuration=RELEASE");

        ctx.Analyzer.Build(options);

        var package = new DirectoryInfo(Path.Combine(ctx.Location.Directory!.FullName, "../artifacts"))
            .EnumerateFiles("*.nupkg", SearchOption.AllDirectories)
            .First();

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

    [Test]
    public void Does_not_add_content_with_SonarQubeIntegration_disabled()
    {
        using var ctx = BuildalyzerContext.ForProject("CompliantCSharpPackage/CompliantCSharpPackage.csproj");

        var options = new EnvironmentOptions() { DesignTime = false };
        options.Arguments.Add("-p:SonarQubeIntegration=false");

        var build = ctx.Analyzer.Build(options);

        var result = build.Results.Single();

        result.Should().HaveItems(
            "Content",
            new ProjectItem()
            {
                ItemSpec = """../../design/logo_128x128.png""",
                Metadata = new Meta { Link = "logo_128x128.png" },
            });
    }
}
