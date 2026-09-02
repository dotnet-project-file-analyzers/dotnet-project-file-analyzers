using DotNetProjectFile.Analyzers.Helpers;

namespace Rules.RoslynRules_specs;

public class NonConfigurables
{
    [Test]
    public void Is_identical_to_Catalog()
    {
        var catalog = DotNetProjectFile.RuleCatalog.DiagnosticCollection.Embedded();
        var rules = catalog.Rules.Where(r => r.NotConfigurable).ToArray();
        var ids = rules.Select(r => r.Id.ToString()).ToHashSet();
#if DEBUG
        Console.WriteLine(string.Join(';', ids.Order()));
#endif
        RoslynRules.NotConfigurables.Should().HaveCount(ids.Count)
            .And.AllSatisfy(id => ids.Contains(id));
    }
}
