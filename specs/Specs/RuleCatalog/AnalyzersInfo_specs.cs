using DotNetProjectFile.RuleCatalog;
using DotNetProjectFile.RuleCatalog.Reflection;
using NuGet.Versioning;
using System.IO;
using System.Reflection;

namespace RuleCatalog.AnalyzersInfo_specs;

public class Embedded
{
    [Test]
    public void Contains_rules()
    {
        var info = DiagnosticCollection.Embedded();
        info.Packages.Should().AllSatisfy(p => p.Rules.Should().NotBeEmpty(because: p.Id));
        info.Count.Should().BeInRange(7000, 8000);
    }
}

public class Collects
{
    [Test]
    public void Aalyzers()
    {
        using var stream = new FileStream(typeof(DotNetProjectFile.Rule).Assembly.Location, FileMode.Open, FileAccess.Read);
        using var loader = new DiagnosticAnalyzersLoader();
        var analyzers = loader.Load(stream);
        analyzers.Should().HaveCountGreaterThan(100);
    }

    [Test]
    [Explicit("Long running process that alters the embedded resource")]
    public async Task New_rules()
    {
        var info = DiagnosticCollection.Embedded();
        info = await DiagnosticCollector.Collect(info);
        info = UpdateDotNetProjectFileAnalyzers(info);

        var file = new DirectoryInfo("../../../../../src/DotNetProjectFile.RuleCatalog/Data/DiagnosticCollection.json");
        using var stream = new FileStream(file.FullName, FileMode.Create);

        info.Save(stream);
        info.Packages.Should().AllSatisfy(p => p.Rules.Should().NotBeEmpty(p.Id));
    }

    private static DiagnosticCollection UpdateDotNetProjectFileAnalyzers(DiagnosticCollection collection)
    {
        var version = new NuGetVersion(typeof(DotNetProjectFile.Rule).Assembly.GetName().Version!);
        version = new(version.Major, version.Minor, version.Patch);

        var package = collection.Packages.Single(p => p.Id == "DotNetProjectFile.Analyzers");
        var rules = package.Rules.ToDictionary(r => r.Id, r => r);

        foreach (var rule in DotNetProjectFileRules().Select(DiagnosticInfo.New))
        {
            rules[rule.Id] = rules.TryGetValue(rule.Id, out var existing)
                ? existing.Update(rule)
                : (rule with
                {
                    Version = version,
                    Languages = [LanguageNames.CSharp, LanguageNames.VisualBasic],
                });
        }

        var updated = package with 
        {
            Version = version,
            Rules = [.. rules.Values],
        };

        return collection with { Packages = collection.Packages.Replace(package, updated) };
    }

    private static IEnumerable<DiagnosticDescriptor> DotNetProjectFileRules()
    {
        Type[] types = [typeof(DotNetProjectFile.Rule), .. typeof(DotNetProjectFile.Rule).GetNestedTypes()];

        return types.SelectMany(t => t.GetProperties(BindingFlags.Public | BindingFlags.Static))
            .Where(p => p.PropertyType == typeof(DiagnosticDescriptor))
            .Select(p => p.GetValue(null))
            .OfType<DiagnosticDescriptor>();
    }

}
