using Buildalyzer.Environment;
using System.IO;

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
    public void Not_when_the_SonarQube_integration_is_disabled()
    {
        using var ctx = BuildalyzerContext.ForProject("SonarQubeIntegration/SonarQubeIntegration.csproj");

        var analyzed = Scanner.FilesToAnalyze(ctx, "-p:SonarQubeIntegration=false");

        analyzed.Should().NotContain(ctx.Location.FullName);
    }
}

/// <summary>Runs a build with a stub of the SonarScanner for .NET targets.</summary>
internal static class Scanner
{
    public static string[] FilesToAnalyze(BuildalyzerContext ctx, params string[] arguments)
    {
        var directory = ctx.Location.Directory!;
        var temp = directory.CreateSubdirectory(Path.Combine("obj", "sonar"));
        var stub = Path.Combine(directory.Parent!.FullName, "SonarQubeScanner.stub.targets");

        var options = new EnvironmentOptions() { DesignTime = false };
        options.Arguments.Add($"-p:SonarQubeTempPath={temp.FullName}");
        options.Arguments.Add($"-p:CustomAfterMicrosoftCommonTargets={stub}");
        foreach (var argument in arguments)
        {
            options.Arguments.Add(argument);
        }

        var result = ctx.Analyzer.Build(options).Results.Single();
        result.Succeeded.Should().BeTrue("the scanner only writes its files to analyze on a successful build");

        return File.ReadAllLines(Path.Combine(temp.FullName, "FilesToAnalyze.txt"));
    }
}
