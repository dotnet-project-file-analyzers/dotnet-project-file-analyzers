using DotNetProjectFile.NuGet.Packaging;
using Specs.TestTools;

namespace NuGet.Nuspec_file_specs;

public class Loads
{
    [Test]
    public void from_stream()
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
      <group targetFramework="".NETStandard2.0"">
        <dependency id=""NuGet.Common"" version=""6.13.1"" exclude=""Build,Analyzers"" />
        <dependency id=""System.Security.Cryptography.ProtectedData"" version=""4.4.0"" exclude=""Build,Analyzers"" />
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

        specs.Should().Be(new NuSpecFile
        {
            Metadata = new()
            {
                Id = "NuGet.Configuration",
                Version = "6.13.1",
                Authors = "Microsoft",
                Description = "NuGet's configuration settings implementation.",
                License = "Apache-2.0",
            }
        });

    }
}
