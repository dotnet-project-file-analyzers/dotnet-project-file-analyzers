using DotNetProjectFile.Analyzers.Helpers;

namespace Rules.RoslynRules_specs;

public class Sets_match_DiagnosticCollection_for
{
    [Test]
    public void NotConfigurables()
    {
        var catalog = DotNetProjectFile.RuleCatalog.DiagnosticCollection.Embedded();
        var rules = catalog.Rules.Where(r => r.NotConfigurable).ToArray();
        var ids = rules.Select(r => r.Id.ToString()).ToHashSet();
#if DEBUG
        Console.WriteLine(string.Join(';', ids.Order()));
#endif
        RoslynRules.NotConfigurables.Should().BeSameSet(ids);
    }

    [Test]
    public void Dropped()
    {
        var catalog = DotNetProjectFile.RuleCatalog.DiagnosticCollection.Embedded();
        var rules = catalog.Rules.Where(r => r.Dropped).ToArray();
        var ids = rules.Select(r => r.Id.ToString()).ToHashSet();
#if DEBUG
        Console.WriteLine(string.Join(';', ids.Order()));
#endif
        RoslynRules.Dropped.Should().BeSameSet(ids);
    }
}
