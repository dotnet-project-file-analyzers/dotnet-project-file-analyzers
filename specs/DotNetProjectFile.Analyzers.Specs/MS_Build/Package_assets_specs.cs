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
    [Explicit]
#endif
    public void Package()
    {
        using var ctx = BuildalyzerContext.ForProject("CompliantCSharpPackage/CompliantCSharpPackage.csproj");

        var options = new EnvironmentOptions() { DesignTime = false };
        options.Arguments.Add("-p:GeneratePackageOnBuild=true");

        ctx.Analyzer.Build(options);

        var package = Path.Combine(ctx.Location.Directory!.FullName, "bin/Debug/CompliantCSharpPackage.1.0.0.nupkg");

        Nupkg.Read(new(package)).Should().Contain([
            "logo_128x128.png",
            "README.md",
            "build/CompliantCSharpPackage.props",
            "build/CompliantCSharpPackage.targets"]);
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
