using DotNetProjectFile.IO;
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
    public void have_mark_down_documentation(Rule rule)
    {
        var file = new FileInfo(Path.Combine(Docs_Readme.Directory!.FullName, "rules", rule.Id + ".md"));
        file.Exists.Should().BeTrue();

        var text = file.OpenText().ReadToEnd();
        text.Should().Contain($"# {rule.Id}: ");
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
    public void are_mentioned_in_docs_README(Rule rule)
    {
        Docs_Readme_Text
            .Contains($"(rules/{rule.Id}.md)")
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

    private static readonly IEnumerable<Type> Types
        = typeof(MsBuildProjectFileAnalyzer).Assembly
        .GetTypes()
        .Where(t => !t.IsAbstract && t.IsAssignableTo(typeof(DiagnosticAnalyzer)));

    private static readonly IEnumerable<Rule> AllRules = GetRules(
        typeof(DotNetProjectFile.Rule),
        typeof(DotNetProjectFile.Rule.Ini));

    private static IEnumerable<Rule> GetRules(params IEnumerable<Type> types) => types
        .SelectMany(t => t.GetProperties(BindingFlags.Public | BindingFlags.Static))
        .Where(f => f.PropertyType == typeof(DiagnosticDescriptor))
        .Select(f => (DiagnosticDescriptor)f.GetValue(null)!)
        .Select(d => new Rule(d));

    private static readonly FileInfo Docs_Readme = new("../../../../../docs/README.md");
    private static readonly FileInfo Package_Readme = new("../../../../../src/DotNetProjectFile.Analyzers/README.md");
    private static readonly FileInfo Root_Readme = new("../../../../../README.md");

    private readonly string Docs_Readme_Text = Docs_Readme.OpenText().ReadToEnd();
    private readonly string Package_Readme_Text = Package_Readme.OpenText().ReadToEnd();
    private readonly string Root_Readme_Text = Root_Readme.OpenText().ReadToEnd();
    
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
            var severity = rule.Descriptor.IsEnabledByDefault
                ? GetIniSeverity(rule.Descriptor.DefaultSeverity)
                : GetIniSeverity(DiagnosticSeverity.Hidden);

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

    private static string GetIniSeverity(DiagnosticSeverity severity) => severity switch
    {
        DiagnosticSeverity.Hidden /*.*/ => "none      ",
        DiagnosticSeverity.Info /*...*/ => "suggestion",
        DiagnosticSeverity.Error /*..*/ => "error     ",
        _ /*.........................*/ => "warning   ",
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

public partial class Documents
{
    private static readonly IEnumerable<IOFile> Files
        = IODirectory.Parse("../../../../../docs/")
        .Files("**/*")!
        .ToArray();

    private static readonly IEnumerable<IOFile> MarkdownFiles
        = Files
        .Where(static file => file.Extension.ToLowerInvariant() == ".md")
        .ToArray();

    [TestCaseSource(nameof(MarkdownFiles))]
    public void Have_permalink(IOFile file)
    {
        TryGetPermalink(file).Should().NotBeNullOrWhiteSpace();
    }

    [TestCaseSource(nameof(MarkdownFiles))]
    public void Have_unique_permalink(IOFile file)
    {
        if (TryGetPermalink(file) is not { } permalink)
        {
            return;
        }

        var others = MarkdownFiles
            .Except([file])
            .Select(static f => TryGetPermalink(f))
            .OfType<string>()
            .Where(static str => !string.IsNullOrWhiteSpace(str))
            .ToHashSet();

        others.Should().NotContain(permalink);
    }

    private static Dictionary<string, string> ParseHeader(string content)
    {
        var result = new Dictionary<string, string>();
        var start = content.IndexOf("---");

        if (start == -1)
        {
            return result;
        }

        var postStart = start + 3;

        var end = content.LastIndexOf("---");

        if (end <= postStart)
        {
            return result;
        }

        var headerLength = end - postStart;
        var headerContent = content.Substring(postStart, headerLength);

        var lines = headerContent.Split('\n');

        foreach (var line in lines)
        {
            var parts = line.Split(':');
            if (parts.Length == 2)
            {
                var key = parts[0].Trim();
                var value = parts[1].Trim();
                result[key] = value;
            }
        }

        return result;
    }

    private static Dictionary<string, string> ParseHeader(in IOFile file)
        => ParseHeader(file.ReadAllText());

    private static string? TryGetPermalink(in IOFile file)
    {
        var header = ParseHeader(file);
        if (header.TryGetValue("permalink", out var result))
        {
            return result;
        }
        else
        {
            return null;
        }
    }
}

