using DotNetProjectFile.NuGet.Packaging;

namespace NuGet.Nuspec_file_specs;

public class Loads
{
    [Test]
    public void v2011()
    {
        using var stream = Streams.FromText(@"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd"">
  <metadata>
    <id>DotNetProjectFile.Analyzers</id>
    <version>1.2.2</version>
    <authors>Corniel Nobel,Wesley Baartman</authors>
    <developmentDependency>true</developmentDependency>
    <license type=""file"">license.txt</license>
    <licenseUrl>https://licenses.nuget.org/MIT</licenseUrl>
    <icon>logo_128x128.png</icon>
    <readme>README.md</readme>
    <projectUrl>https://www.github.com/Corniel/dotnet-project-file-analyzers</projectUrl>
    <iconUrl>https://raw.githubusercontent.com/Corniel/dotnet-project-file-analyzers/main/design/logo_128x128.png</iconUrl>
    <description>.NET project file analyzers</description>
    <copyright>Copyright © Corniel Nobel 2023-current</copyright>
    <tags>Code Analysis Project files csproj vbproj resx MS Build resources</tags>
    <repository type=""git"" url=""https://www.github.com/Corniel/dotnet-project-file-analyzers"" commit=""cd0017da4a7eb2a2f765cbe821dcb14745e9aaeb"" />
  </metadata>
</package>");

        var specs = NuSpecFile.Load(stream);

        specs.Should().Be(new NuSpecFile
        {
            Metadata = new()
            {
                Id = "DotNetProjectFile.Analyzers",
                Version = "1.2.2",
                License = new() { Type = "file", Value = "license.txt" },
                LicenseUrl = "https://licenses.nuget.org/MIT",
                DevelopmentDependency = true,
            }
        });
    }

    [Test]
    public void v2013()
    {
        using var stream = Streams.FromText(@"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd"">
  <metadata>
    <id>NuGet.Configuration</id>
    <version>6.13.1</version>
    <authors>Microsoft</authors>
    <requireLicenseAcceptance>true</requireLicenseAcceptance>
    <license type=""expression"">Apache-2.0</license>
    <licenseUrl>https://licenses.nuget.org/Apache-2.0</licenseUrl>
    <icon>icon.png</icon>
    <readme>README.md</readme>
    <projectUrl>https://aka.ms/nugetprj</projectUrl>
    <description>NuGet's configuration settings implementation.</description>
    <copyright>© Microsoft Corporation. All rights reserved.</copyright>
    <tags>nuget</tags>
    <serviceable>true</serviceable>
    <repository type=""git"" url=""https://github.com/NuGet/NuGet.Client"" commit=""3fd0e588e53525f0cd037d7b91174c0ca78ac65c"" />
    <dependencies>
      <group targetFramework="".NETFramework4.7.2"">
        <dependency id=""NuGet.Common"" version=""6.13.1"" exclude=""Build,Analyzers"" />
      </group>
    </dependencies>
    <frameworkAssemblies>
      <frameworkAssembly assemblyName=""System.Security"" targetFramework="".NETFramework4.7.2"" />
      <frameworkAssembly assemblyName=""System.Xml"" targetFramework="".NETFramework4.7.2"" />
      <frameworkAssembly assemblyName=""System.Xml.Linq"" targetFramework="".NETFramework4.7.2"" />
    </frameworkAssemblies>
  </metadata>
</package>");

        var specs = NuSpecFile.Load(stream);


        specs.Should().BeEquivalentTo(new NuSpecFile
        {
            Metadata = new()
            {
                Id = "NuGet.Configuration",
                Version = "6.13.1",
                License = new() { Type = "expression", Value = "Apache-2.0" },
                LicenseUrl = "https://licenses.nuget.org/Apache-2.0",
                Dependencies =
                [
                    new()
                    {
                        TargetFramework = ".NETFramework4.7.2",
                        Dependencies = [new() { Id = "NuGet.Common", Version ="6.13.1", Exclude = "Build,Analyzers" } ]
                    }
                ],
            },
        });
    }
}
