using Buildalyzer.Environment;
using System.IO;
using System.Reflection;
using static Specs.TestTools.TestPath;

namespace MS_Build.SonarQube_integration_specs;

[NonParallelizable] // these tests all build the shared SonarQubeIntegration fixture (BuildalyzerContext
// wipes its bin/obj), so running them in parallel makes the builds trip over each other.
public class Registers_AdditionalFiles
{
    [Test]
    public void As_item_type_to_analyze()
    {
        using var ctx = BuildalyzerContext.ForProject("SonarQubeIntegration/SonarQubeIntegration.csproj");

        var result = ctx.Analyzer.Build().Results.Single();

        result.Should().HaveProperties(new()
        {
            ["SonarQubeIntegration"] = "true",
            ["SQAdditionalAnalysisFileItemTypes"] = ";AdditionalFiles",
        });
    }

    [Test]
    public void So_that_the_scanner_analyzes_the_project_file()
    {
        using var ctx = BuildalyzerContext.ForProject("SonarQubeIntegration/SonarQubeIntegration.csproj");

        var analyzed = Scanner.FilesToAnalyze(ctx);

        analyzed.Should().Contain(ctx.Location.FullName);
    }

    [Test]
    public void Next_to_the_files_the_scanner_collects_itself()
    {
        using var ctx = BuildalyzerContext.ForProject("SonarQubeIntegration/SonarQubeIntegration.csproj");

        var analyzed = Scanner.FilesToAnalyze(ctx);

        analyzed.Should().Contain(Full("common/Code.cs"));
    }

    [Test]
    public void Not_when_the_SonarQube_integration_is_disabled()
    {
        using var ctx = BuildalyzerContext.ForProject("SonarQubeIntegration/SonarQubeIntegration.csproj");

        var analyzed = Scanner.FilesToAnalyze(ctx, "-p:SonarQubeIntegration=false");

        analyzed.Should().NotContain(ctx.Location.FullName);
    }
}

/// <summary>Runs a build with the MSBuild targets of SonarScanner for .NET.</summary>
internal static class Scanner
{
    /// <remarks>
    /// The scanner points $(CustomAfterMicrosoftCommonTargets) to its own targets file, and
    /// collects the files to analyze in a project specific directory below $(SonarQubeTempPath).
    /// </remarks>
    public static string[] FilesToAnalyze(BuildalyzerContext ctx, params string[] arguments)
    {
        var temp = ctx.Location.Directory!.CreateSubdirectory(Path.Combine("obj", "sonar"));

        var options = new EnvironmentOptions() { DesignTime = false };
        options.Arguments.Add($"-p:SonarQubeTempPath={temp.FullName}");
        options.Arguments.Add($"-p:CustomAfterMicrosoftCommonTargets={Targets}");

        foreach (var argument in arguments)
        {
            options.Arguments.Add(argument);
        }

        var result = ctx.Analyzer.Build(options).Results.Single();
        result.Succeeded.Should().BeTrue("the scanner only collects its files to analyze on a successful build");

        var collected = temp.EnumerateFiles("FilesToAnalyze.txt", SearchOption.AllDirectories).ToArray();
        collected.Should().ContainSingle("the scanner collects the files to analyze once per project");

        return File.ReadAllLines(collected.Single().FullName);
    }

    /// <summary>The targets file of the downloaded SonarScanner for .NET package.</summary>
    private static string Targets => field ??= Directory
        .EnumerateFiles(Package, "SonarQube.Integration.targets", SearchOption.AllDirectories)
        .Single();

    private static string Package => typeof(Scanner).Assembly
        .GetCustomAttributes<AssemblyMetadataAttribute>()
        .Single(m => m.Key == "SonarScanner").Value!;
}
