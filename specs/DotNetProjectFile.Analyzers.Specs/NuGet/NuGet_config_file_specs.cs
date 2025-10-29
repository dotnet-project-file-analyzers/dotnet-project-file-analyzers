using DotNetProjectFile.NuGet.Packaging;

namespace NuGet.NuGet_config_file_specs;

public class Loads
{
    [Test]
    public void X()
    {
        using var stream = Streams.FromText(@"
<configuration>
    <config>
        <add key=""globalPackagesFolder"" value=""C:\nuget\packages"" />
    </config>
    <packageSources>
        <clear />
        <add key=""NuGet.org"" value=""https://api.nuget.org/v3/index.json""/>
        <!-- Use only one of the following public feeds depending on your geographic region. -->
        <add key=""ServerArtifactoryNuGet"" value=""https://relativitypackageseastus.jfrog.io/artifactory/api/nuget/v3/server-nuget-virtual"" />
        <!-- <add key=""ServerArtifactoryNuGet"" value=""https://relativitypackageswesteurope.jfrog.io/artifactory/api/nuget/v3/server-nuget-virtual"" /> /-->
    </packageSources>
    <packageSourceMapping>
        <!-- ServerArtifactoryNuGet is the preferred source because it has a more specific pattern.
             NuGet.org is a fallback for other packages such as NUnit. -->
        <packageSource key=""ServerArtifactoryNuGet"">
            <package pattern=""Relativity.Server.*"" />
        </packageSource>
        <packageSource key=""NuGet.org"">
            <package pattern=""*"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");

        var config = NuGetConfigFile.Load(stream);

        config.Should().BeEquivalentTo(new NuGetConfigFile
        {
            Configs =
            [
                new()
                {
                    KeyValues = [new() { Key = "globalPackagesFolder", Value = "C:\\nuget\\packages" }]
                }
            ],
        });
    }
}
