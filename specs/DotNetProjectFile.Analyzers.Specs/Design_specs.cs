using System.Reflection;

namespace Design_specs;

public class Rules
{
    [Test]
    public void Ids_are_unique()
        => Descriptors.Select(d => d.Id).Should().OnlyHaveUniqueItems();

    [TestCaseSource(nameof(RuleIds))]
    public void have_mark_down_documentation(string id)
        => new FileInfo($"../../../../../rules/{id}.md").Exists.Should().BeTrue(because: $"{id}.md should exist.");

    [TestCaseSource(nameof(RuleIds))]
    public void mentioned_in_README_root(string id)
        => ReadmeRootText
        .Contains($"](rules/{id}.md)")
        .Should().BeTrue(because: $"Rule {id} should be mentioned");

    [TestCaseSource(nameof(RuleIds))]
    public void mentioned_in_README_package(string id)
        => ReadmePackageText
        .Contains($"](https://github.com/Corniel/dotnet-project-file-analyzers/blob/main/rules/{id}.md)")
        .Should().BeTrue(because: $"Rule {id} should be mentioned");

    [TestCaseSource(nameof(Types))]
    public void in_DotNetProjectFile_Analyzers_MsBuild_namespace(Type type)
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

    private static IEnumerable<DiagnosticDescriptor> Descriptors
        => typeof(DotNetProjectFile.Rule)
        .GetProperties(BindingFlags.Public | BindingFlags.Static)
        .Where(f => f.PropertyType == typeof(DiagnosticDescriptor))
        .Select(f => (DiagnosticDescriptor)f.GetValue(null)!);

    private static readonly string[] RuleIds = Descriptors.Select(d => d.Id).ToArray();

    private static readonly FileInfo ReadmeRoot = new("../../../../../README.md");
    private static readonly FileInfo ReadmePackage = new("../../../../../src/DotNetProjectFile.Analyzers/README.md");

    private readonly string ReadmeRootText = ReadmeRoot.OpenText().ReadToEnd();
    private readonly string ReadmePackageText = ReadmePackage.OpenText().ReadToEnd();
}

