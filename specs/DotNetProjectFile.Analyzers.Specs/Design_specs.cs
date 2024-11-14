using DotNetProjectFile.CodeAnalysis;
using DotNetProjectFile.IO;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Design_specs;

public partial class Rules
{
    [Test]
    public void have_unique_ids()
        => AlleRules.Should().OnlyHaveUniqueItems(d => d.Id);

    [TestCaseSource(nameof(AlleRules))]
    public async Task have_mark_down_documentation(Rule rule)
    {
        using var client = new HttpClient();
        
        var response = await client.GetAsync(rule.HelpLinkUri.Replace(".html", ".md"));
        response.Should().HaveStatusCode(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        content.Should().StartWith($"# {rule.Id}: ");
    }

    [Test]
    public void Root_Readme_mentions_right_number_of_rules()
    {
        var match = AmountPattern().Match(Root_Readme_Text);
        var amount = int.Parse(match.Groups["amount"].Value);

        var all = AlleRules.Count();

        (amount / 10).Should().Be(all / 10, because: $"Mentioned {amount}+ should be approximately {all}.");
    }

    [TestCaseSource(nameof(AlleRules))]
    public async Task are_mentioned_in_Web_README(Rule rule)
    {
        Web_Index_Context ??= await GetWebIndexContext();
        Web_Index_Context
            .Contains(@$"""/rules/{rule.Id}.html""")
            .Should().BeTrue(because: $"Rule {rule.Id} should be mentioned");
    }

    [TestCaseSource(nameof(AlleRules))]
    public void are_mentioned_in_README_package(Rule rule)
        => Package_Readme_Text
        .Contains($"]({rule.HelpLinkUri})")
        .Should().BeTrue(because: $"Rule {rule} should be mentioned");

    [TestCaseSource(nameof(Types))]
    public void defined_in_DotNetProjectFile_Analyzers_MsBuild_namespace(Type type)
        => type.Namespace.Should().BeOneOf(
            "DotNetProjectFile.Analyzers.Generic",
            "DotNetProjectFile.Analyzers.MsBuild",
            "DotNetProjectFile.Analyzers.Resx");

    [TestCaseSource(nameof(Types))]
    public void Has_supported_diagnostics(Type type)
        => ((DiagnosticAnalyzer)Activator.CreateInstance(type)!).SupportedDiagnostics.Should().NotBeEmpty();

    [TestCaseSource(nameof(Types))]
    public void for_CSharp_and_VB(Type type)
        => type.GetCustomAttribute<DiagnosticAnalyzerAttribute>()!
        .Languages.Should().BeEquivalentTo("C#", "Visual Basic");

    private static IEnumerable<Type> Types
        => typeof(MsBuildProjectFileAnalyzer).Assembly
        .GetTypes()
        .Where(t => !t.IsAbstract && t.IsAssignableTo(typeof(DiagnosticAnalyzer)));

    private static IEnumerable<Rule> AlleRules
        => typeof(DotNetProjectFile.Rule)
        .GetProperties(BindingFlags.Public | BindingFlags.Static)
        .Where(f => f.PropertyType == typeof(DiagnosticDescriptor))
        .Select(f => (DiagnosticDescriptor)f.GetValue(null)!)
        .Select(d => new Rule(d));

    private static readonly FileInfo Root_Readme = new("../../../../../README.md");
    private static readonly FileInfo Package_Readme = new("../../../../../src/DotNetProjectFile.Analyzers/README.md");

    private readonly string Root_Readme_Text = Root_Readme.OpenText().ReadToEnd();
    private readonly string Package_Readme_Text = Package_Readme.OpenText().ReadToEnd();
    private string? Web_Index_Context;

    private static async Task<string> GetWebIndexContext()
    {
        using var client = new HttpClient(); 
        var response = await client.GetAsync("https://dotnet-project-file-analyzers.github.io/");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    [GeneratedRegex(@"Contains (?<amount>[0-9]+0)\+ \[Roslyn\]")]
    private static partial Regex AmountPattern();
}


public class SDK
{
    [Test]
    public void references_latest_version()
    {
        var proj = ProjectFiles.Global.MsBuildProject(IOFile.Parse("../../../../../src/DotNetProjectFile.Analyzers/DotNetProjectFile.Analyzers.csproj"));
        var prop = ProjectFiles.Global.MsBuildProject(IOFile.Parse("../../../../../src/DotNetProjectFile.Analyzers.Sdk/DotNetProjectFile.Analyzers.Sdk.props"));

        var vers = proj!.PropertyGroups.SelectMany(p => p.Version).Last().Value;
        var reff = prop!.ItemGroups.SelectMany(p => p.PackageReferences).Single(p => p.Include == "DotNetProjectFile.Analyzers").Version;
        vers.Should().NotBeEmpty();
        reff.Should().Be(vers);
    }
}

/// <remarks>Wrapper for better display of test resources in IDE.</remarks>
public sealed record Rule(DiagnosticDescriptor Descriptor)
{
    public string Id => Descriptor.Id;

    public string HelpLinkUri => Descriptor.HelpLinkUri;

    public override string ToString() => Descriptor.Id;
}
