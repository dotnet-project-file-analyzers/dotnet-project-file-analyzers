using Buildalyzer.Environment;
using System.IO;
using System.Reflection;
using static Specs.TestTools.TestPath;

namespace MS_Build.SonarQube_integration_specs;

[NonParallelizable] // both tests build the same fixture, of which BuildalyzerContext wipes bin/obj.
public class Registers_AdditionalFiles
{
    [Test]
    public void So_that_the_scanner_analyzes_the_project_file()
    {
        using var ctx = BuildalyzerContext.ForProject("SonarQubeIntegration/SonarQubeIntegration.csproj");

        Scanner.FilesToAnalyze(ctx).Should().Contain(ctx.Location.FullName);
    }

    [Test]
    public void Unless_the_SonarQube_integration_is_disabled()
    {
        using var ctx = BuildalyzerContext.ForProject("SonarQubeIntegration/SonarQubeIntegration.csproj");

        Scanner.FilesToAnalyze(ctx, "-p:SonarQubeIntegration=false")
            .Should().Equal(Full("common/Code.cs"));
    }
}

/// <summary>Builds with the MSBuild targets of SonarScanner for .NET.</summary>
internal static class Scanner
{
    /// <remarks>
    /// The scanner is activated by pointing $(CustomAfterMicrosoftCommonTargets) to its targets
    /// file. It collects the files to analyze per project, in a unique directory below
    /// $(SonarQubeTempPath).
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

        ctx.Analyzer.Build(options).Results.Single().Succeeded
            .Should().BeTrue("the scanner only collects its files to analyze on a successful build");

        var collected = temp.EnumerateFiles("FilesToAnalyze.txt", SearchOption.AllDirectories)
            .Should().ContainSingle().Which;

        return File.ReadAllLines(collected.FullName);
    }

    /// <summary>The targets file of the downloaded SonarScanner for .NET package.</summary>
    private static string Targets => Directory
        .EnumerateFiles(Package, "SonarQube.Integration.targets", SearchOption.AllDirectories)
        .Single();

    private static string Package => typeof(Scanner).Assembly
        .GetCustomAttributes<AssemblyMetadataAttribute>()
        .Single(m => m.Key == "SonarScanner").Value!;
}
