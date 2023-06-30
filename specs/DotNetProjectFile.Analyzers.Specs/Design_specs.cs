using Microsoft.CodeAnalysis.Diagnostics;
using System.Reflection;

namespace Design_specs;

public class Rules
{
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
}

