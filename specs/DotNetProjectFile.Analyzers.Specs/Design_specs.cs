using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Reflection;

namespace Design_specs;

public class Rules
{
    [Test]
    public void Ids_are_unique()
        => Descriptors.Select(d => d.Id).Should().OnlyHaveUniqueItems();

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
        => typeof(DotNetProjectFile.ProjectFileAnalyzer).Assembly
        .GetTypes()
        .Where(t => !t.IsAbstract && t.IsAssignableTo(typeof(DiagnosticAnalyzer)));

    public static IEnumerable<DiagnosticDescriptor> Descriptors
        => typeof(DotNetProjectFile.Rule)
        .GetFields(BindingFlags.Public | BindingFlags.Static)
        .Where(f => f.FieldType == typeof(DiagnosticDescriptor))
        .Select(f => (DiagnosticDescriptor)f.GetValue(null)!);
}

