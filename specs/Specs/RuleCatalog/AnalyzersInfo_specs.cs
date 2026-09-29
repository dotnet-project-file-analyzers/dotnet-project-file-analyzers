using DotNetProjectFile.RuleCatalog;
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
    private static readonly FileInfo File = new("../../../../../src/DotNetProjectFile.RuleCatalog/Data/DiagnosticCollection.json")
    private static readonly NuGetVersion Placeholder = new(999, 99, 9);

    [TestCase("1.19.0")]
    [Explicit("Only run this just before shipping a new package")]
    public async Task Set_DotNetProjectFile_Analyzers_version(NuGetVersion version)
    {
        var info = DiagnosticCollection.Embedded();
        info = await DiagnosticCollector.Collect(info);

        var package = info.Packages.Single(p => p.Id == "DotNetProjectFile.Analyzers");
        var updated = package with
        {
            Version = version,
            Rules = [.. package.Rules.Select(r => r.Version == Placeholder ? r with { Version = version } : r)],
        };

        info = info with { Packages = info.Packages.Replace(package, updated) };

        using var stream = new FileStream(File.FullName, FileMode.Create);
        info.Save(stream);
        info.Packages.Should().AllSatisfy(p => p.Rules.Should().NotBeEmpty(p.Id));
    }

    [Test]
    [Explicit("Long running process that alters the embedded resource")]
    public async Task New_rules()
    {
        var info = DiagnosticCollection.Embedded();
        info = await DiagnosticCollector.Collect(info);
        info = UpdateDotNetProjectFileAnalyzers(info);

        using var stream = new FileStream(File.FullName, FileMode.Create);
        info.Save(stream);
        info.Packages.Should().AllSatisfy(p => p.Rules.Should().NotBeEmpty(p.Id));
    }

    private static DiagnosticCollection UpdateDotNetProjectFileAnalyzers(DiagnosticCollection collection)
    {
        var package = collection.Packages.Single(p => p.Id == "DotNetProjectFile.Analyzers");
        var rules = package.Rules.ToDictionary(r => r.Id, r => r);

        foreach (var rule in DotNetProjectFileRules().Select(DiagnosticInfo.New))
        {
            rules[rule.Id] = rules.TryGetValue(rule.Id, out var existing)
                ? existing.Update(rule)
                : (rule with
                {
                    First = Placeholder,
                    Languages = [LanguageNames.CSharp, LanguageNames.VisualBasic],
                });
        }

        var updated = package with 
        {
            Version = Placeholder,
            Rules = [.. rules.Values.Order()],
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
