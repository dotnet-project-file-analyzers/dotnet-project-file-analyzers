using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using VerifyNUnit;
using VerifyTests;

namespace Design_specs;

public partial class Rules
{
    [Test]
    public void have_unique_ids()
        => AllRules.Should().OnlyHaveUniqueItems(d => d.Id);

    [TestCaseSource(nameof(AllRules))]
    public async Task have_mark_down_documentation(Rule rule)
    {
        using var client = new HttpClient();
        var response = await client.GetAsync(rule.HelpLinkUri);
        response.StatusCode.Should().Be(HttpStatusCode.OK, rule.HelpLinkUri);

        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain($">{rule.Id}: ");
    }

    /// <summary>
    /// The diagnostic message should not contain any line return character nor any leading or trailing whitespaces and should either be a single sentence without a trailing period or a multi-sentences with a trailing period
    /// </summary>
    /// <remarks>
    /// See: https://github.com/dotnet/roslyn-analyzers/blob/main/src/Microsoft.CodeAnalysis.Analyzers/Microsoft.CodeAnalysis.Analyzers.md#rs1032-define-diagnostic-message-correctly
    /// </remarks>
    [TestCaseSource(nameof(AllRules))]
    public void has_message_compliant_with_RS1032(Rule rule)
    {
        var title = rule.Message.ToString().Trim();

        title.Should().NotContain("\n", "Line return characters are not allowed.")
            .And.NotEndWith(".", "Be a single line not adding with a trailing dot.");
    }

    [Test]
    public void Root_Readme_mentions_right_number_of_rules()
    {
        var match = AmountPattern().Match(Root_Readme_Text);
        var amount = int.Parse(match.Groups["amount"].Value);

        var all = AllRules.Count();
#if DEBUG
        Console.WriteLine($"Contains {all} rules.");
#endif
        (amount / 10).Should().Be(all / 10, because: $"Mentioned {amount}+ should be approximately {all}.");
    }

    [TestCaseSource(nameof(AllRules))]
    public async Task are_mentioned_in_Web_README(Rule rule)
    {
        Web_Index_Context ??= await GetWebIndexContext();
        Web_Index_Context
            .Contains(@$"""/rules/{rule.Id}.html""")
            .Should().BeTrue(because: $"Rule {rule.Id} should be mentioned");
    }

    [TestCaseSource(nameof(AllRules))]
    public void are_mentioned_in_README_package(Rule rule)
        => Package_Readme_Text
        .Contains($"]({rule.HelpLinkUri})")
        .Should().BeTrue(because: $"Rule {rule} should be mentioned");

    [TestCaseSource(nameof(Types))]
    public void defined_in_DotNetProjectFile_Analyzers_namespace(Type type)
        => type.Namespace!.StartsWith("DotNetProjectFile.Analyzers.");

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

    private static IEnumerable<Rule> AllRules => GetRules(
        typeof(DotNetProjectFile.Rule),
        typeof(DotNetProjectFile.Rule.Ini));

    private static IEnumerable<Rule> GetRules(params IEnumerable<Type> types) => types
        .SelectMany(t => t.GetProperties(BindingFlags.Public | BindingFlags.Static))
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

    [Test]
    public async Task Globalconfig()
    {
        var config = GetGlobalconfigContent();

        var settings = new VerifySettings();
        settings.UseDirectory(GetOutputPath());
        settings.UseFileName("globalconfig");
        settings.AutoVerify(fileName => !File.Exists(fileName));
        VerifierSettings.UseUtf8NoBom();

        await Verifier.Verify(config, settings);
    }

    private static string GetOutputPath([CallerFilePath] string? path = null)
    {
        var curDir = path is null
            ? Directory.GetCurrentDirectory()
            : Path.GetDirectoryName(path) ?? Directory.GetCurrentDirectory();

        var outputDir = Path.Combine(curDir, "../../");
        Directory.CreateDirectory(outputDir);
        return outputDir;
    }

    private static string GetGlobalconfigContent()
    {
        var sb = new StringBuilder();

        var rules = AllRules.OrderBy(x => x.Id).ToArray();

        foreach (var rule in rules)
        {
            var id = rule.Id;
            var severity = GetIniSeverity(rule.Descriptor.DefaultSeverity);
            var title = rule.Descriptor.Title.ToString(CultureInfo.InvariantCulture);

            sb
                .Append("dotnet_diagnostic.")
                .Append(id)
                .Append(".severity = ")
                .Append(severity)
                .Append(" # ")
                .AppendLine(title);
        }

        return sb.ToString();
    }

    private static string GetIniSeverity(DiagnosticSeverity severity)
        => severity switch
        {
            DiagnosticSeverity.Hidden /**/ => "none      ",
            DiagnosticSeverity.Info /****/ => "suggestion",
            DiagnosticSeverity.Error /***/ => "error     ",
            _ /**************************/ => "warning   ",
        };
}

/// <remarks>Wrapper for better display of test resources in IDE.</remarks>
public sealed record Rule(DiagnosticDescriptor Descriptor)
{
    public string Id => Descriptor.Id;

    public LocalizableString Title => Descriptor.Title;

    public LocalizableString Message => Descriptor.MessageFormat;

    public string HelpLinkUri => Descriptor.HelpLinkUri;

    public override string ToString() => Descriptor.Id;
}
