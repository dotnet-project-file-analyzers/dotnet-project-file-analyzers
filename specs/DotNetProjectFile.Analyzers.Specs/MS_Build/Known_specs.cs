using DotNetProjectFile.MsBuild;

namespace MS_Build.Known_specs;

public class NodeNames
{
    private static readonly IEnumerable<string> All = Known.NodeNames;

    [TestCaseSource(nameof(All))]
    public void do_not_node_names_for_existing_nodes(string name)
        => Node.Factory.KnownNodes.Contains(name).Should().BeFalse(name);

#if DEBUG
#else
    [Ignore("")]
#endif
    [Test]
    public void Renders()
    {
        var all = string.Join(",\r\n", Known.NodeNames.Select(x => $@"""{x}""")
            .Order());

        Console.WriteLine(all);
    }
}
