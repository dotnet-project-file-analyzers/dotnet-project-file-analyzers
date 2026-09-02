using DotNetProjectFile.RuleCatalog;
using System.IO;

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

[Explicit("Long running process that alters the embedded resource")]
public class Collects
{
    [Test]
    public async Task New_rules()
    {
        var info = DiagnosticCollection.Embedded();
        info = await DiagnosticCollector.Collect(info);

        var file = new DirectoryInfo("../../../../../src/DotNetProjectFile.RuleCatalog/Data/DiagnosticCollection.json");
        using var stream = new FileStream(file.FullName, FileMode.Create);

        info.Save(stream);
        info.Packages.Should().AllSatisfy(p => p.Rules.Should().NotBeEmpty(p.Id));
    }
}
