using System.Net;
using System.Reflection;

namespace Design_specs;

public class Rules
{
    [Test]
    public void have_unique_ids()
        => AlleRules.Select(d => d.Id).Should().OnlyHaveUniqueItems();

    [TestCaseSource(nameof(AlleRules))]
    public async Task have_mark_down_documentation(Rule rule)
    {
        using var client = new HttpClient();
        
        var response = await client.GetAsync(rule.HelpLinkUri.Replace(".html", ".md"));
        response.Should().HaveStatusCode(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        content.Should().StartWith($"# {rule.Id}: ");
    }

    [TestCaseSource(nameof(AlleRules))]
    public async Task are_mentioned_in_README_root(Rule rule)
    {
        IndexContext ??= await GetIndexContext();
        IndexContext
            .Contains($@"""/rules/{rule.Id}.html""")
            .Should().BeTrue(because: $"Rule {rule} should be mentioned");
    }

    [TestCaseSource(nameof(AlleRules))]
    public void are_mentioned_in_README_package(Rule rule)
        => ReadmePackageText
        .Contains($"]({rule.HelpLinkUri})")
        .Should().BeTrue(because: $"Rule {rule} should be mentioned");

    [TestCaseSource(nameof(Types))]
    public void defined_in_DotNetProjectFile_Analyzers_MsBuild_namespace(Type type)
        => type.Namespace.Should().BeOneOf(
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

    private static readonly FileInfo ReadmePackage = new("../../../../../src/DotNetProjectFile.Analyzers/README.md");

    private readonly string ReadmePackageText = ReadmePackage.OpenText().ReadToEnd();


    private string? IndexContext;

    private static async Task<string> GetIndexContext()
    {
        using var client = new HttpClient(); 
        var response = await client.GetAsync("https://dotnet-project-file-analyzers.github.io/");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}

/// <remarks>Wrapper for better display of test resources in IDE.</remarks>
public sealed record Rule(DiagnosticDescriptor Descriptor)
{
    public string Id => Descriptor.Id;
    public string HelpLinkUri => Descriptor.HelpLinkUri;

    public override string ToString() => Descriptor.Id;
}
