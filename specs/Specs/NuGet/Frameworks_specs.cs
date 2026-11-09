using DotNetProjectFile.NuGet;
using DotNetProjectFile.NuGet.Packaging;

namespace Target_frameworks_specs;

public class Parses
{
    [TestCase("net10.0", "netcoreapp", "10.0")]
    [TestCase("net8.0-windows", "netcoreapp", "8.0")]
    [TestCase("netcoreapp3.1", "netcoreapp", "3.1")]
    [TestCase(".NETCoreApp3.1", "netcoreapp", "3.1")]
    [TestCase("net472", "netframework", "4.7.2")]
    [TestCase("net48", "netframework", "4.8")]
    [TestCase(".NETFramework4.6.2", "netframework", "4.6.2")]
    [TestCase("netstandard2.0", "netstandard", "2.0")]
    [TestCase(".NETStandard2.1", "netstandard", "2.1")]
    public void known_frameworks(string tfm, string family, string version)
        => Frameworks.Parse(tfm).Should().Be(new Frameworks.Framework(family, Version.Parse(version)));

    [TestCase(null)]
    [TestCase("")]
    [TestCase("MonoAndroid10")]
    [TestCase("$(TargetFrameworks)")]
    public void unknown_as_null(string? tfm) => Frameworks.Parse(tfm).Should().BeNull();
}

public class Nearest
{
    [TestCase("net10.0", "net8.0")]
    [TestCase("net6.0", "net6.0")]
    [TestCase("net5.0", "netstandard2.1")]
    [TestCase("net472", "net462")]
    [TestCase("net461", "netstandard2.0")]
    [TestCase("netstandard2.0", "netstandard2.0")]
    public void picks_same_family_over_netstandard(string project, string expected)
    {
        string[] groups = ["net462", "netstandard2.0", "netstandard2.1", "net6.0", "net8.0", "net11.0"];
        var index = Frameworks.Nearest(Tfm(project), [.. groups.Select(Frameworks.Parse)]);
        groups[index].Should().Be(expected);
    }

    [Test]
    public void none_when_incompatible()
        => Frameworks.Nearest(Tfm("net462"), [Frameworks.Parse("net8.0")]).Should().Be(-1);

    private static Frameworks.Framework Tfm(string tfm) => Frameworks.Parse(tfm) ?? throw new FormatException(tfm);
}

public class Dependencies_for
{
    private static readonly NuspecDependencies Dependencies = new()
    {
        Groups =
        [
            new() { TargetFramework = ".NETFramework4.6.2", Dependencies = [new() { Id = "Legacy", Version = "1.0.0" }] },
            new() { TargetFramework = "net8.0", Dependencies = [new() { Id = "Modern", Version = "8.0.0" }] },
            new() { Dependencies = [new() { Id = "Fallback", Version = "1.0.0" }] },
        ],
    };

    [Test]
    public void nearest_group_only()
        => Dependencies.For(["net10.0"]).Select(d => d.Id).Should().BeEquivalentTo("Modern");

    [Test]
    public void union_for_multiple_target_frameworks()
        => Dependencies.For(["net10.0", "net472"]).Select(d => d.Id).Should().BeEquivalentTo("Modern", "Legacy");

    [Test]
    public void group_without_framework_as_fallback()
        => Dependencies.For(["netstandard2.0"]).Select(d => d.Id).Should().BeEquivalentTo("Fallback");

    [Test]
    public void all_when_framework_is_unknown()
        => Dependencies.For(["$(Tfm)"]).Select(d => d.Id).Should().BeEquivalentTo("Legacy", "Modern", "Fallback");
}
