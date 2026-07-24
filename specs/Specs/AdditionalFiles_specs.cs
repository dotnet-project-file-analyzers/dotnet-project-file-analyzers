using Buildalyzer;
using static Specs.TestTools.TestPath;
using Meta = Specs.TestTools.ProjectItem.Meta;
using ProjectItem = Specs.TestTools.ProjectItem;

namespace AdditionalFiles_specs;

public class Resolves
{
    [Test]
    public void For_Project()
    {
        using var ctx = BuildalyzerContext.ForProject("AdditionalFilesProject/AdditionalFilesProject/AdditionalFilesProject.csproj");

        var result = ctx.Analyzer.Build().Results.Single();
        Log(result);

        result.Should().HaveAdditionalFiles(

            new ProjectItem()
            {
                ItemSpec = Full("AdditionalFilesProject/AdditionalFilesProject/AdditionalFilesProject.csproj"),
                Metadata = new Meta
                {
                    AnalyzerType = "MSBuildProject",
                    Visible = "false",
                }
            },

            new ProjectItem()
            {
                ItemSpec = Full("AdditionalFilesProject/Directory.Build.props"),
                Metadata = new Meta
                {
                    AnalyzerType = "DirectoryBuildProps",
                    Visible= "false",
                    Link = "Directory.Build.props",
                }
            },
            
            new ProjectItem()
            {
                ItemSpec = Full("AdditionalFilesProject/Directory.Build.targets"),
                Metadata = new Meta
                {
                    AnalyzerType = "DirectoryBuildTargets",
                    Visible = "false",
                    Link = "Directory.Build.targets",
                }
            },

            new ProjectItem()
            {
                ItemSpec = Full("AdditionalFilesProject/Directory.Packages.props"),
                Metadata = new Meta
                {
                    AnalyzerType = "DirectoryPackagesProps",
                    Visible = "false",
                    Link = "Directory.Packages.props",
                }
            },

            new ProjectItem
            {
                ItemSpec = "Resources.resx",
                Metadata = new Meta
                {
                    AnalyzerType = "RESX",
                },
            });
    }

    [Test]
    public void For_SDK()
    {
        using var ctx = BuildalyzerContext.ForProject("AdditionalFilesProject/.net.csproj");

        var result = ctx.Analyzer.Build().Results.Single();

        Log(result);

        result.Should().HaveAdditionalFiles(

            new ProjectItem
            {
                ItemSpec = ".net.csproj",
                Metadata = new Meta
                {
                    AnalyzerType = "SDK",
                    Visible = "false",
                },
            },
            new ProjectItem
            {
                ItemSpec = "additional-files.slnx",
                Metadata = new Meta
                {
                    Visible = "false",
                    AnalyzerType = "SLNX",
                },
            },
            new ProjectItem
            {
                ItemSpec = Relative("AdditionalFilesProject/AdditionalFilesProject.csproj"),
                Metadata = new Meta
                {
                    Visible = "false",
                    AnalyzerType = "MSBuildProject",
                },
            },
            new ProjectItem
            {
                ItemSpec = Full("../.editorconfig"),
                Metadata = new Meta
                {
                    Link = ".editorconfig",
                    AnalyzerType = "EditorConfig",
                },
            },
            new ProjectItem
            {
                ItemSpec = Full("../.globalconfig"),
                Metadata = new Meta
                {
                    Link = ".globalconfig",
                    AnalyzerType = "GlobalConfig",
                },
            },
            new ProjectItem
            {
                ItemSpec = Full("../NuGet.config"),
                Metadata = new Meta
                {
                    Link = "NuGet.config",
                    AnalyzerType = "NuGetConfig",
                },
            },
            new ProjectItem
            {
                ItemSpec = "copyright.props",
                Metadata = new Meta
                {
                    Visible = "true",
                    AnalyzerType = "MSBuildProps",
                },
            },
            new ProjectItem
            {
                ItemSpec = "Directory.Build.props",
                Metadata = new Meta
                {
                    Visible = "true",
                    AnalyzerType = "MSBuildProps",
                },
            },
            new ProjectItem
            {
                ItemSpec = "Directory.Build.targets",
                Metadata = new Meta
                {
                    Visible = "true",
                    AnalyzerType = "MSBuildProps",
                },
            },
            new ProjectItem
            {
                ItemSpec = "Directory.Packages.props",
                Metadata = new Meta
                {
                    Visible = "true",
                    AnalyzerType = "MSBuildProps",
                },
            });
    }

    [Test]
    public void For_Blazor_scoped_css()
    {
        using var ctx = BuildalyzerContext.ForProject("BlazorScopedCss/BlazorScopedCss.csproj");

        var result = ctx.Analyzer.Build().Results.Single();
        Log(result);

        result.Should().HaveContent(
             new ProjectItem
             {
                 ItemSpec = Full("BlazorScopedCss/BlazorScopedCss.csproj"),
                 Metadata = new Meta
                 {
                     CopyToOutputDirectory = "never",
                     Link = Link(Full("BlazorScopedCss/BlazorScopedCss.csproj")),
                     Visible = "false",
                     SonarQubeContent = "true",
                     AnalyzerType = "MSBuildProject",
                 },
             },
            new ProjectItem
            {
                ItemSpec = Relative("Components/Tree.razor"),
                Metadata = new Meta().Set("ExcludeFromSingleFile", "true"),
            }
        );

        result.Should().HaveAdditionalFiles(
             new ProjectItem
             {
                 ItemSpec = Full("BlazorScopedCss/BlazorScopedCss.csproj"),
                 Metadata = new Meta
                 {
                     Visible = "false",
                     AnalyzerType = "MSBuildProject",
                 },
             }
        );
    }

    private static void Log(IAnalyzerResult result)
    {
#if DEBUG
        ProjectItem.Generate(result.Items.OfType("Content"));
#endif
    }
}
