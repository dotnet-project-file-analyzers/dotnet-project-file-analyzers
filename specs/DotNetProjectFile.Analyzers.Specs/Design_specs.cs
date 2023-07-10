using System.Reflection;

namespace Design_specs;

public class Rules
{
    [Test]
    public void Ids_are_unique()
        => Descriptors.Select(d => d.Id).Should().OnlyHaveUniqueItems();

    [TestCaseSource(nameof(Descriptors))]
    public void have_mark_down_documentation(DiagnosticDescriptor descriptor)
        => new FileInfo($"../../../../../rules/{descriptor.Id:0000}.md").Exists.Should().BeTrue(because: $"{descriptor.Id:0000}.md should exist.");

    [TestCaseSource(nameof(Types))]
    public void in_DotNetProjectFile_Analyzers_namespace(Type type)
        => type.Namespace.Should().Be("DotNetProjectFile.Analyzers");

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

    public static IEnumerable<DiagnosticDescriptor> Descriptors
        => typeof(DotNetProjectFile.Rule)
        .GetProperties(BindingFlags.Public | BindingFlags.Static)
        .Where(f => f.PropertyType == typeof(DiagnosticDescriptor))
        .Select(f => (DiagnosticDescriptor)f.GetValue(null)!)
        .ToArray();
}

