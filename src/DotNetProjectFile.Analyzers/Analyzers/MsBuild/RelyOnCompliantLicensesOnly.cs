using NuGet.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class RelyOnCompliantLicensesOnly() : MsBuildProjectFileAnalyzer(Rule.RelyOnCompliantLicensesOnly)
{
    protected override void Register(ProjectFileAnalysisContext<MsBuildProject> context)
    {
        var settings = global::NuGet.Configuration.Settings.LoadDefaultSettings(context.File.Path.Directory.ToString());
        var packageFolder = IODirectory.Parse(SettingsUtility.GetGlobalPackagesFolder(settings));

        foreach (var package in context.File.Walk().OfType<PackageReference>())
        {
            try
            {
                var path = packageFolder.SubDirectory(package.Include?.ToLowerInvariant(), package.Version?.ToLowerInvariant());

                if (path.Files("*.nuspec").FirstOrDefault() is { HasValue: true } nuspec)
                {

                    using var stream = nuspec.OpenRead();
                    var reader = new global::NuGet.Packaging.NuspecReader(stream);
                    var license = reader.GetLicenseMetadata();

                    context.ReportDiagnostic(Descriptor, package, package.Include, license.License);
                }
            }
            catch (Exception) { }
        }
    }
}
