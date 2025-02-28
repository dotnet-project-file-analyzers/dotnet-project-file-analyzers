using DotNetProjectFile.Diagnostics;
using DotNetProjectFile.Licensing;
using DotNetProjectFile.MsBuild;
using DotNetProjectFile.NuGet;

namespace Licensing.File_specs;

public class Can_be_determined
{
    [TestCase("Ardalis.ApiEndpoints", "4.1.0", "MIT")]
    [TestCase("Fractions", "7.3.0", "BSD-2-Clause")]
    [TestCase("J2N", "2.1.0", "Apache-2.0")]
    public void For(string packageId, string version, string expectedLicense)
    {
        var analyzer = new Analyzer();
        analyzer.ForInlineCsproj(@$"
            <Project Sdk=""Microsoft.NET.Sdk"">

              <PropertyGroup>
                <TargetFramework>net8.0</TargetFramework>
              </PropertyGroup>

              <ItemGroup>
                <PackageReference Include=""{packageId}"" Version=""{version}"" />
              </ItemGroup>

            </Project>
        ")
        .HasNoIssues();

        analyzer.Detected[packageId].Should().Be(Licenses.FromExpression(expectedLicense));
    }

    [DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
    private sealed class Analyzer() : MsBuildProjectFileAnalyzer(new("XXX", "XXX", "XXX", "XXX", DiagnosticSeverity.Warning, true))
    {
        public Dictionary<string, LicenseExpression> Detected { get; } = new(StringComparer.OrdinalIgnoreCase);

        protected override void Register(ProjectFileAnalysisContext<DotNetProjectFile.MsBuild.Project> context)
        {
            var references = context.File.ItemGroups
                .SelectMany(g => g.Children)
                .OfType<PackageReferenceBase>()
                .ToArray();
            foreach (var reference in references)
            {
                if (PackageCache.GetPackage(reference.IncludeOrUpdate, reference.Version) is { } pkg)
                {
                    Detected[pkg.Name] = pkg.License;
                }
            }
        }
    }
}

public class Can_not_be_determined
{
    [TestCase("MIT", "6oH4uPyx6N+SGpjv3f37ng")]
    [TestCase("Apache-2.0", "wnT4A3LZDAEpNzcPDh8VCA")]
    public void Custom_license_snapshots(string baseLicense, string expectedHash)
    {
        var baseText = (Licenses.FromExpression(baseLicense) as SingleLicense)?.SpdxInfo?.LicenseTexts.FirstOrDefault() ?? string.Empty;
        var resolved = CustomLicense.Create(baseText);
        resolved.Hash.Should().Be(expectedHash);
    }
}
